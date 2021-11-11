using System;

namespace DakarRally.Core.DTO.ReadObjects
{
    public class ReadVehicleStatisticsObject
    {
        public int CoveredDistance { get; set; }

        public string[] MalfunctionStatistics { get; set; }

        public string Status { get; set; }

        public TimeSpan FinishTime { get; set; }
    }
}
