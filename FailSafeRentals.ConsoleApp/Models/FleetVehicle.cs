using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FailSafeRentals.ConsoleApp.Models
{
    public class FleetVehicle
    {
        public int Id{ get; set; }

        [Required, MaxLength(50)]
        public string RegistrationNumber { get; set; } = null!;

        //[Required, MaxLength(100)]
        //public string MakeAndModel { get; set; } = null!;
        [Required, MaxLength(40)]
        public string Make { get; set; } = string.Empty;

        [Required, MaxLength(40)]
        public string Model { get; set; } = string.Empty;

        [Required]
        public int MileageKm{ get; set; }
        public bool NeedsService { get; set; } = false;

        // Rental station och Fleet vehicle är en en-till-många relation så vi behöver en kopplingstabell.
        // Explicit främmande nyckel konvention public int \<Tabellnamn>Id { get; set; }
        public int RentalStationId { get; set; }
        public RentalStation RentalStation{ get; set; }

        // Lägg till DamageReports Explicit
        public int DamageReportId { get; set; }
        public List<DamageReport> DamageReports { get; set; } = null!;

        // Ett fordon kan ha många underhållstaggar
        public List<MaintenanceTag> MaintenanceTags { get; set; } = [];
    }
}
