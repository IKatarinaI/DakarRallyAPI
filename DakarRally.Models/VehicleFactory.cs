using DakarRally.Models;
using System;
using static DakarRally.Models.Enums;

namespace DakarRally.Core.Entities
{
    public class VehicleFactory : IVehicleFactory
    {
        public DrivingSimulation CreateVehicle(Enums.VehicleClass _class)
        {
            switch (_class)
            {
                case VehicleClass.Car:
                    {
                        return new Car();
                    }
                case VehicleClass.Motorbike:
                    {
                        return new Motorbike();
                    }
                case VehicleClass.Truck:
                    {
                        return new Truck();
                    }
                default:
                    throw new ArgumentException();
            }
        }
    }
}
