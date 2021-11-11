using System;
using System.Collections.Generic;

namespace DakarRally.DTO.ReadObjects
{
    public class ReadRaceObject
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public string Status { get; set; }
        public ICollection<ReadVehicleObject> Vehicles { get; set; }
    }
}
