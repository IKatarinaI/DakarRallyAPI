using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DakarRally.Models.Enums;

namespace DakarRally.API.Validators
{
    public class VehicleValidator : IValidatableObject
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

        public RaceStatus RaceStatus { get; set; }

        [Required]
        public int RaceYear { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Class == VehicleClass.Truck && Type != null)
            {
                yield return new ValidationResult(
                    "Vehicle of class Truck does not have any type.",
                    new[] { "UpdateVehicleObject" });
            }

            if (Class == VehicleClass.Car && (!Type.HasValue || Type.Value == VehicleType.Cross))
            {
                yield return new ValidationResult(
                    "Vehicle of class Car can not be of type Cross or empty.",
                    new[] { "UpdateVehicleObject" });
            }

            if (Class == VehicleClass.Motorbike && (!Type.HasValue || Type.Value == VehicleType.Terrain))
            {
                yield return new ValidationResult(
                    "Vehicle of class Motorbike can not be of type Terrain or empty.",
                    new[] { "UpdateVehicleObject" });
            }

            if (RaceStatus != RaceStatus.Pending)
            {
                yield return new ValidationResult(
                    "Operations on vehicle can not performed once the race is started or finished.",
                    new[] { "Vehicle" });
            }

            if (RaceYear < ManufacturingDate.Year)
            {
                yield return new ValidationResult(
                    "Vehicles date of manufacturing can not be greater than the year of the race.",
                    new[] { "Vehicle" });
            }
        }
    }
}
