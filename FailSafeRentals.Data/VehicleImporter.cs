using FailSafeRentals.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FailSafeRentals.Data
{
    public  class VehicleImporter(FailSafeRentalsDbContext _db)
    {
        public void ImportFromTableFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var existingRegNumbers = _db.Vehicles
                .Select(v => v.RegistrationNumber.ToUpper())
                .ToHashSet();

            var seenInFile = new HashSet<string>();
            var validVehicles = new List<Vehicle>();
            var errors = new List<string>();

            // Börja på index 1 för att hoppa över rubrik raden
            for (int i = 1; i < lines.Length; i++)
            {
                int rowNumber = i + 1;
                var parts = lines[i].Split('\t'); // Splittar på backslash t = tab tecken
                if (parts.Length < 3)
                {
                    errors.Add($"Rad {rowNumber}: För få kolumner");
                    continue;
                }

                string regNr = parts[0].Trim().ToUpper();
                string brand = parts[1].Trim();
                //string model = parts[2].Trim();
                string rateRaw = parts[2].Trim();

                // Validera regnr & dubbletter
                if (string.IsNullOrWhiteSpace(regNr) || regNr.Length > 10)
                {
                    errors.Add($"Rad {rowNumber}: Ogiltligt regnr: {regNr}");
                    continue;
                }
                if (existingRegNumbers.Contains(regNr) || !seenInFile.Add(regNr))
                {
                    errors.Add($"Rad {rowNumber}: Regnr finns redan: {regNr}");
                    continue;
                }
                // 14-9 fällan: InvariantCulture krävs för decimalpunkt
                if (!decimal.TryParse(rateRaw, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal dailyRate))
                {
                    errors.Add($"Rad {rowNumber}: Ogiltligt pris: {rateRaw}");
                    continue;
                }
                // Kommer vi hit så är raden juste
                validVehicles.Add(new Vehicle
                {
                    RegistrationNumber = regNr,
                    Brand = brand,
                    Model = " ",
                    DailyRate = dailyRate,
                    IsAvailable = true
                });
            }
            _db.Vehicles.AddRange(validVehicles);
            _db.SaveChanges();

            Console.WriteLine();
            Console.WriteLine($"Import klar! Godkända: {validVehicles.Count} | Avvisade: {errors.Count}");
            foreach (var item in errors)
            {
                Console.WriteLine(item);
            }
        }
    }
}
