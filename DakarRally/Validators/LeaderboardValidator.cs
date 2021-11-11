using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DakarRally.Models.Enums;

namespace DakarRally.API.Validators
{
    public class LeaderboardValidator : IValidatableObject
    {
        public RaceStatus Status { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Status == RaceStatus.Pending)
            {
                yield return new ValidationResult(
                    "Leaderboard for race that has not been started does not exist.",
                    new[] { "Leaderboard" });
            }
        }
    }
}

