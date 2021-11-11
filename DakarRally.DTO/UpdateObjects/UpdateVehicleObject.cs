using System;
using System.ComponentModel.DataAnnotations;
using static DakarRally.Models.Enums;

namespace DakarRally.DTO.UpdateObjects
{
    public class UpdateVehicleObject
    {
        [Required]
        public string TeamName { get; set; }

        [Required]
        public string ModelName { get; set; }

        [Required]
        public DateTime ManufacturingDate { get; set; }

        [Required]
        public VehicleClass Class { get; set; }

        public VehicleType? Type { get; set; }
    }
}
