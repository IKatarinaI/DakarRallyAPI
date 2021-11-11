using DakarRally.DTO.CreateObjects;
using NUnit.Framework;
using System.Linq;
using static DakarRally.Models.Enums;

namespace DakarRally.Tests.DakarRally.Core.DTO.Test
{
    [TestFixture]

    public class CreateVehicleObjectTest
    {
        [TestCase(VehicleClass.Truck, VehicleType.Cross)]
        [TestCase(VehicleClass.Car, VehicleType.Cross)]
        [TestCase(VehicleClass.Motorbike, VehicleType.Terrain)]
        public void ValidateForVehicleForCreationWithInvalidInput_ReturnsValidationResult(VehicleClass vClass, VehicleType? vType)
        {
            var classUnderTest = new CreateVehicleObject() { Class = vClass, Type = vType };
            var result = classUnderTest.Validate(null).ToList();

            Assert.IsTrue(result.Count == 1);
        }

        [TestCase(VehicleClass.Truck, null)]
        [TestCase(VehicleClass.Car, VehicleType.Sport)]
        [TestCase(VehicleClass.Motorbike, VehicleType.Cross)]
        public void ValidateForVehicleForCreationWithValidInput_ReturnsNoValidationResult(VehicleClass vClass, VehicleType? vType)
        {
            var classUnderTest = new CreateVehicleObject() { Class = vClass, Type = vType };
            var result = classUnderTest.Validate(null).ToList();

            Assert.IsTrue(result.Count == 0);
        }
    }
}

