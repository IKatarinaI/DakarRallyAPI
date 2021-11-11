using System.ComponentModel.DataAnnotations;
using static DakarRally.Models.Enums;

namespace DakarRally.DTO.UpdateObjects
{
    public class UpdateRaceObject
    {
        [Required]
        public RaceStatus Status { get; set; }
    }
}
