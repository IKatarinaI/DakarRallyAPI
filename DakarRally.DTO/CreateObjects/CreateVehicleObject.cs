using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DakarRally.Models.Enums;

namespace DakarRally.DTO.CreateObjects
{
    public class CreateVehicleObject: IValidatableObject
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Class == VehicleClass.Truck && Type != null)
            {
                yield return new ValidationResult(
                    "Vehicle of class Truck does not have any type.",
                    new[] { "CreateVehicleObject" });
            }

            if (Class == VehicleClass.Car && (!Type.HasValue || Type.Value == VehicleType.Cross))
            {
                yield return new ValidationResult(
                    "Vehicle of class Car can't be of type Cross or empty.",
                    new[] { "CreateVehicleObject" });
            }

            if (Class == VehicleClass.Motorbike && (!Type.HasValue || Type.Value == VehicleType.Terrain))
            {
                yield return new ValidationResult(
                    "Vehicle of class Motorbike can't be of type Terrain or empty.",
                    new[] { "CreateVehicleObject" });
            }
        }
    }
}
