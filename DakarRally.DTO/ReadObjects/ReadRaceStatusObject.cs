using System.Collections.Generic;

namespace DakarRally.Core.DTO.ReadObjects
{
    public class ReadRaceStatusObject
    {
        public string Status { get; set; }

        public Dictionary<string, int> NumberOfVehiclesByStatus = new Dictionary<string, int>()
        {
            { "Ready", 0 },
            { "Racing", 0 },
            { "Malfunctioned", 0 },
            { "Finished", 0 }
        };

        public Dictionary<string, int> NumberOfVehiclesByClass = new Dictionary<string, int>()
        {
            { "Car", 0 },
            { "Motorbike", 0 },
            { "Truck", 0 }
        };
    }
}
