using System;
using System.Collections.Generic;
using System.Text;

namespace FailSafeRentals.ConsoleApp.Models
{
    public class RentalStation
    {
        public int Id { get; set; }
        public string StationCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string StreetAddress { get; set; } = null!;

        // En rental station har många fleet vehicles
        public List<FleetVehicle> Vehicles { get; set; } = [];
    }
}
