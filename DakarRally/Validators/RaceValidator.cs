using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DakarRally.Models.Enums;

namespace DakarRally.API.Validators
{
    public class RaceValidator : IValidatableObject
    {
        [Required]
        public RaceStatus Status { get; set; }

        public RaceStatus OldStatus { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Status != RaceStatus.Running || OldStatus != RaceStatus.Pending)
            {
                yield return new ValidationResult(
                    "Status of the race can only be changed to Running if it was previously in Pending status.",
                    new[] { "UpdateRaceObject" });
            }
        }
    }
}
