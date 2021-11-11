using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DakarRally.DTO.CreateObjects
{
    public class CreateRaceObject
    {
        [Required]
        public int Year { get; set; }
    }
}
