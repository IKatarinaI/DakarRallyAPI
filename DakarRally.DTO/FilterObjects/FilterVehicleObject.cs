using System;
using static DakarRally.Models.Enums;

namespace DakarRally.Core.DTO.FilterObjects
{
    public class FilterVehicleObject
    {
        public string TeamName { get; set; }

        public string ModelName { get; set; }

        public DateTime? ManufacturingDate { get; set; }

        public VehicleStatus? Status { get; set; }

        public int? CoveredDistance { get; set; }
    }
}

