using DakarRally.Core.DTO.FilterObjects;
using DakarRally.Models;
using System;
using System.Collections.Generic;
using static DakarRally.Models.Enums;

namespace DakarRally.Infrastructure.Repositories
{
    public interface IDakarRallyRepository
    {
        IEnumerable<Race> GetRaces();
        Race GetRace(Guid raceId);
        bool RaceExists(Guid raceId);
        void AddRace(Race newRace);
        bool AnyRaceInProgress();
        IEnumerable<Vehicle> GetVehiclesForRace(Guid raceId, FilterVehicleObject filterVehicleObject = null);
        IEnumerable<Vehicle> GetVehiclesForRace(Guid raceId, VehicleClass vClass);
        Vehicle GetVehicleForRace(Guid raceId, Guid vehicleId);
        void AddVehicle(Guid raceId, Vehicle newVehicle);
        void RemoveVehicle(Vehicle vehicle);
        void UpdateVehicle(Vehicle vehicle);
        bool Save();
    }
}
