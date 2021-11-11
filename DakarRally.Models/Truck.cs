using DakarRally.Models;
using System.Threading;

namespace DakarRally.Core.Entities
{
    public class Truck : DrivingSimulation
    {
        public Truck()
            : base()
        {
            this.MaxSpeed = Constants.TruckCarsMaxSpeed;
            this.HeavyMalfunction = Constants.TruckCarsHeavyMalfunction;
            this.LightMalfunction = Constants.TruckCarsLightMalfunction;
        }

        public override void Repair()
        {
            Thread.Sleep(Constants.TruckRepairTime * 1000);
        }
    }
}


