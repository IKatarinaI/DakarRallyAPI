using static DakarRally.Models.Enums;

namespace DakarRally.Core.Entities
{
    public interface IVehicleFactory
    {
        public DrivingSimulation CreateVehicle(VehicleClass _class);
    }
}
