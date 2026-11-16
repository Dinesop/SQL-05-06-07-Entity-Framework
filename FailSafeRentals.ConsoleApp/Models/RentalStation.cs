using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FailSafeRentals.ConsoleApp.Models
{
    public class RentalStation
    {
        public int Id { get; set; }

        [Required, MaxLength(10)]
        public string StationCode { get; set; } = null!;

        [Required, MaxLength(50)]
        public string City { get; set; } = null!;

        [Required, MaxLength(250)]
        public string StreetAddress { get; set; } = null!;

        // En rental station har många fleet vehicles
        public List<FleetVehicle> Vehicles { get; set; } = [];
    }
}
