using DakarRally.Models;
using System.Threading;
using static DakarRally.Models.Enums;

namespace DakarRally.Core.Entities
{
    public class Car : DrivingSimulation
    {
        public override void SetType(VehicleType? type)
        {
            this.VehicleType = type;

            switch (type)
            {
                case Models.Enums.VehicleType.Terrain:
                    this.MaxSpeed = Constants.TerrainCarsMaxSpeed;
                    this.LightMalfunction = Constants.TerrainCarsLightMalfunction;
                    this.HeavyMalfunction = Constants.TerrainCarsHeavyMalfunction;
                    break;
                case Models.Enums.VehicleType.Sport:
                    this.MaxSpeed = Constants.SportCarsMaxSpeed;
                    this.LightMalfunction = Constants.SportCarsLightMalfunction;
                    this.HeavyMalfunction = Constants.SportCarsHeavyMalfunction;
                    break;
            }
        }

        public override void Repair()
        {
            Thread.Sleep(Constants.CarRepairTime * 1000);
        }
    }
}
