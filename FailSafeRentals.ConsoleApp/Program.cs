using FailSafeRentals.Core.Models;
using FailSafeRentals.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

using var _db = new FailSafeRentalsDbContext();

#region 
//if (!_db.Vehicles.Any())
//{
//    var gps = _db.ExtraFeatures.FirstOrDefault(f => f.Name == "GPS Navigation")
//              ?? new ExtraFeature { Name = "GPS Navigation", DailyPrice = 75m };

//    var car = new Vehicle
//    {
//        RegistrationNumber = "ABC123",
//        Brand = "Volvo",
//        Model = "XC60",
//        DailyRate = 895m,
//        ExtraFeatures = new() { gps }
//    };

//    var customer = new Customer
//    {
//        FullName = "Anna Lindqvist",
//        Email = "anna@failsafe.se",
//        DriverLicenseNumber = "SE-998877"
//    };

//    _db.Rentals.Add(new Rental
//    {
//        Customer = customer,
//        Vehicle = car,
//        StartDate = DateTime.UtcNow,
//        TotalCost = 2685m
//    });

//    _db.SaveChanges();
//}

//var rental = _db.Rentals
//    .Include(r => r.Customer)
//    .Include(r => r.Vehicle)
//    .First();

//Console.WriteLine($"Verifierat: {rental.Customer.FullName} hyr {rental.Vehicle.Brand} ({rental.Vehicle.RegistrationNumber})");
#endregion

#region
//SkapaCustomer();
//SkapaFleraFordon();
//HämtaAllaTillgängligaFordon();
// HämtaEttSpecifiktFordon();
//UppdateraVehicle();
//TaBortVehicle();
//LäggTillUthyrningar();
//HämtaUthyrningar();
#endregion

#region
//if (Debugger.IsAttached)
//{
//    Console.Write("\n\nTryck på en tangent för att stänga fönstret...");
//    Console.ReadKey();
//}
#endregion






void SkapaCustomer()
{
    using var _db = new FailSafeRentalsDbContext(); // handtag till databasen

    // Video 6
    var newCustomer = new Customer
    {
        FullName = "Astrid Lindholm",
        Email = "astrid.lindholm@failsafe.se",
        DriverLicenseNumber = "SE-123456789"
    };
    _db.Customers.Add(newCustomer);
    _db.SaveChanges(); // sparar till databasen.
    Console.WriteLine($"[Info] Kund sparad med Id: {newCustomer.Id}");

    // Uppgift 1
    var customer = new Customer
    {
        FullName = "Maja Lindberg",
        Email = "maja.lindberg@failsafe.se",
        DriverLicenseNumber = "SE-123456789"
    };

    _db.Customers.Add(customer);
    _db.SaveChanges(); // Flyttar infon från EF Core till SQL Server
    Console.WriteLine($"[Info] Kund sparad med Id: {customer.Id}");
}
void SkapaFleraFordon()
{
    using var _db = new FailSafeRentalsDbContext(); // handtag till databasen

    var fleetBatch = new List<Vehicle>
    {
        new(){RegistrationNumber = "FS-001", Brand = "Volvo", Model = "V90", DailyRate = 850m, IsAvailable = true},
        new() { RegistrationNumber = "FS-002", Brand = "Polestar", Model = "2", DailyRate = 920m, IsAvailable = true },
        new() { RegistrationNumber = "FS-003", Brand = "Tesla", Model = "Model 3", DailyRate = 980m, IsAvailable = false },
        new() { RegistrationNumber = "FS-004", Brand = "Volkswagen", Model = "ID.4", DailyRate = 760m, IsAvailable = true },
        new() { RegistrationNumber = "FS-005", Brand = "Kia", Model = "EV6", DailyRate = 790m, IsAvailable = true }
    };
    _db.Vehicles.AddRange(fleetBatch);
    _db.SaveChanges();
}
void HämtaAllaTillgängligaFordon()
{
    using var _db = new FailSafeRentalsDbContext(); // handtag till databasen

    List<Vehicle> availableVehicles = _db.Vehicles
        .Where(v => v.IsAvailable).ToList();
    foreach (var vehicle in availableVehicles)
    {
        Console.WriteLine($"{vehicle.Brand} {vehicle.Model} {vehicle.DailyRate} {vehicle.IsAvailable}");
    }

    // Uppgift 2
    var availableVehicles2 = _db.Vehicles
    .Where(v => v.IsAvailable)
    .OrderByDescending(v => v.DailyRate)
    .ToList();

    foreach (var v in availableVehicles2)
    {
        Console.WriteLine($"{v.Brand} {v.Model} {v.RegistrationNumber} is available | Cost: {v.DailyRate:C}/day");
    }
}
void HämtaEttSpecifiktFordon()
{
    using var _db = new FailSafeRentalsDbContext(); // handtag till databasen

    Vehicle? vehicle = _db.Vehicles.Find(1); //vi anger primärnyckeln i find
    if (vehicle is not null)
    {
        Console.WriteLine($"Hittade {vehicle.Brand} {vehicle.Model} {vehicle.RegistrationNumber}");
    }
}
void UppdateraVehicle()
{
    using var _db = new FailSafeRentalsDbContext(); // handtag till databasen

    // UPDATE: Hämta, ändra egenskap, spara
    var vehicleToUpdate = _db.Vehicles.Find(1);
    if (vehicleToUpdate is not null) { vehicleToUpdate.DailyRate = 750m; _db.SaveChanges(); Console.WriteLine($"{vehicleToUpdate.Brand} {vehicleToUpdate.Model} has rate updated to {vehicleToUpdate.DailyRate}:-/h"); }
}
void TaBortVehicle()
{
    using var _db = new FailSafeRentalsDbContext(); // handtag till databasen
    
    // DELETE: Hämta, Markera för borttagning, Spara
    var vehicleToUpdate = _db.Vehicles.Find(99);
    if (vehicleToUpdate is not null) { _db.Vehicles.Remove(vehicleToUpdate); _db.SaveChanges(); Console.WriteLine("Tog bort fordon 99."); }
    else { Console.WriteLine("Fordon 99 hittades inte."); }

}
void LäggTillUthyrningar()
{
    using var _db = new FailSafeRentalsDbContext(); // handtag till databasen
    
    var uthyrningar = new List<Rental>
    {
            new() { CustomerId = 1, VehicleId = 2, StartDate = DateTime.UtcNow, TotalCost = 1685m },
            new() { CustomerId = 1, VehicleId = 3, StartDate = DateTime.UtcNow, TotalCost = 2685m },
            new() { CustomerId = 1, VehicleId = 4, StartDate = DateTime.UtcNow, TotalCost = 3685m },
            new() { CustomerId = 1, VehicleId = 5, StartDate = DateTime.UtcNow, TotalCost = 4685m },
            new() { CustomerId = 1, VehicleId = 6, StartDate = DateTime.UtcNow, TotalCost = 5685m },

            new() { CustomerId = 2, VehicleId = 2, StartDate = DateTime.UtcNow, TotalCost = 1505m },
            new() { CustomerId = 2, VehicleId = 3, StartDate = DateTime.UtcNow, TotalCost = 2505m },
            new() { CustomerId = 2, VehicleId = 4, StartDate = DateTime.UtcNow, TotalCost = 3505m },
            new() { CustomerId = 2, VehicleId = 5, StartDate = DateTime.UtcNow, TotalCost = 4505m },
            new() { CustomerId = 2, VehicleId = 6, StartDate = DateTime.UtcNow, TotalCost = 5505m },

    };

    _db.Rentals.AddRange(uthyrningar);
    _db.SaveChanges();
}
void HämtaUthyrningar()
{
    using var _db = new FailSafeRentalsDbContext(); // handtag till databasen

    var recentHighValueRentals = _db.Rentals
        .Where(r => r.TotalCost >= 3000m)
        .OrderByDescending(r => r.StartDate)
        .Take(5)
        .ToList();
    Console.WriteLine();
    foreach (var r in recentHighValueRentals)
    { Console.WriteLine($"{r.StartDate} {r.CustomerId} {r.TotalCost}"); }
}