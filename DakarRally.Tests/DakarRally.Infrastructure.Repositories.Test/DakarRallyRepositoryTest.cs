using DakarRally.DBAccess;
using DakarRally.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using static DakarRally.Models.Enums;

namespace DakarRally.Infrastructure.Repositories.Test
{
    [TestFixture]
    public class DakarRallyRepositoryTest
    {
        static string databaseName = "testDB";
        private DbContextOptionsBuilder<DakarRallyContext> optionsBuilder = new DbContextOptionsBuilder<DakarRallyContext>().UseInMemoryDatabase(databaseName);
        private Guid RaceGuid = Guid.NewGuid();
        private Guid CarGuid = Guid.NewGuid();
        private Guid TruckGuid = Guid.NewGuid();
        private Guid MotorbikeGuid = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            using (DakarRallyContext dbContext = new DakarRallyContext(optionsBuilder.Options))
            {
                PopulateDB(dbContext);
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (DakarRallyContext dbContext = new DakarRallyContext(optionsBuilder.Options))
            {
                DropDB(dbContext);
            }
        }

        private void PopulateDB(DakarRallyContext dbContext)
        {
            dbContext.Races.Add(new Race() { Id = RaceGuid, Status = RaceStatus.Pending, Year = 2020 });
            dbContext.Vehicles.Add(new Vehicle()
            {
                Id = CarGuid,
                Class = VehicleClass.Car,
                CoveredDistance = 0,

                ManufacturingDate = DateTime.Now,
                ModelName = "TestModel",
                RaceId = RaceGuid,
                Status = VehicleStatus.Ready,
                TeamName = "TestTeam",
                Type = VehicleType.Sport
            });
            dbContext.Vehicles.Add(new Vehicle()
            {
                Id = TruckGuid,
                Class = VehicleClass.Truck,
                CoveredDistance = 1400,
                ManufacturingDate = DateTime.Now,
                ModelName = "TestModel2",
                RaceId = RaceGuid,
                Status = VehicleStatus.Racing,
                TeamName = "TestTeam2",
                Type = null
            });
            dbContext.Vehicles.Add(new Vehicle()
            {
                Id = MotorbikeGuid,
                Class = VehicleClass.Motorbike,
                CoveredDistance = 4900,
                ManufacturingDate = DateTime.Now,
                ModelName = "TestModel3",
                RaceId = RaceGuid,
                Status = VehicleStatus.Ready,
                TeamName = "TestTeam3",
                Type = VehicleType.Sport
            });

            dbContext.SaveChanges();
        }

        private void DropDB(DakarRallyContext dbContex)
        {
            dbContex.Database.EnsureDeleted();
        }

        [Test]
        public void GetRacesTest()
        {
            using (DakarRallyContext dbContext = new DakarRallyContext(optionsBuilder.Options))
            {
                var classUnderTest = new DakarRallyRepository(dbContext);
                var races = classUnderTest.GetRaces().ToList();

                Assert.IsTrue(races.Count == 1);
                Assert.IsTrue((races[0].Id == RaceGuid) && (races[0].Year == 2020) && (races[0].Status == RaceStatus.Pending));
            }
        }

        [Test]
        public void GetRaceTest_ThrowsArgumentExceptionForInvalidInput()
        {
            using (DakarRallyContext dbContext = new DakarRallyContext(optionsBuilder.Options))
            {
                var classUnderTest = new DakarRallyRepository(dbContext);
                Assert.Throws<ArgumentNullException>(() => classUnderTest.GetRace(Guid.Empty));
            }
        }

        [Test]
        public void AddRaceTest_RaceAddedForValidInput()
        {
            using (DakarRallyContext dbContext = new DakarRallyContext(optionsBuilder.Options))
            {
                var classUnderTest = new DakarRallyRepository(dbContext);
                var raceToAdd = new Race() { Status = RaceStatus.Pending, Year = 1999 };
                classUnderTest.AddRace(raceToAdd);
                var s = classUnderTest.Save();

                Assert.IsTrue(classUnderTest.RaceExists(raceToAdd.Id));
            }
        }

        [Test]
        public void GetVehiclesForRace_ReturnsAllVehiclesForNoParams()
        {
            using (DakarRallyContext dbContext = new DakarRallyContext(optionsBuilder.Options))
            {
                var classUnderTest = new DakarRallyRepository(dbContext);
                var vehicles = classUnderTest.GetVehiclesForRace(RaceGuid).ToList();

                Assert.AreEqual(3, vehicles.Count);
            }
        }

        [Test]
        public void GetVehiclesForRace_WithParams()
        {
            using (DakarRallyContext dbContext = new DakarRallyContext(optionsBuilder.Options))
            {
                var classUnderTest = new DakarRallyRepository(dbContext);
                var vehicles = classUnderTest.GetVehiclesForRace(RaceGuid, new Core.DTO.FilterObjects.FilterVehicleObject() { TeamName = "TestTeam" }).ToList();

                Assert.IsTrue(vehicles.Count == 1);
                Assert.AreEqual(vehicles[0].Id, CarGuid);

                vehicles = classUnderTest.GetVehiclesForRace(RaceGuid, new Core.DTO.FilterObjects.FilterVehicleObject() { TeamName = "TestTeam", ModelName = "TestModel2" }).ToList();
                Assert.IsTrue(vehicles.Count == 0);

                vehicles = classUnderTest.GetVehiclesForRace(RaceGuid, new Core.DTO.FilterObjects.FilterVehicleObject() { Status = VehicleStatus.Ready }).ToList();
                Assert.IsTrue(vehicles.Count == 2);
            }
        }
    }
}