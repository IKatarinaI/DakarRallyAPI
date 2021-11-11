using DakarRally.DTO.ReadObjects;

namespace DakarRally.Core.DTO.ReadObjects
{
    public class ReadVehicleRankObject
    {
        public int Rank { get; set; }

        public ReadVehicleObject Vehicle { get; set; }
    }
}
