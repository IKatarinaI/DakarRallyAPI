using DakarRally.Models;
using System.Threading;
using static DakarRally.Models.Enums;

namespace DakarRally.Core.Entities
{
    public class Motorbike : DrivingSimulation
    {
        public override void SetType(VehicleType? type)
        {
            this.VehicleType = type;

            switch (type)
            {
                case Models.Enums.VehicleType.Cross:
                    this.MaxSpeed = Constants.CrossMotorbikesMaxSpeed;
                    this.LightMalfunction = Constants.CrossMotorbikesLightMalfunction;
                    this.HeavyMalfunction = Constants.CrossMotorbikesHeavyMalfunction;
                    break;
                case Models.Enums.VehicleType.Sport:
                    this.MaxSpeed = Constants.SportMotorbikesMaxSpeed;
                    this.LightMalfunction = Constants.SportsMotorbikesLightMalfunction;
                    this.HeavyMalfunction = Constants.SportsMotorbikesHeavyMalfunction;
                    break;
            }
        }

        public override void Repair()
        {
            Thread.Sleep(Constants.MotorbikeRepairTime * 1000);
        }
    }
}

