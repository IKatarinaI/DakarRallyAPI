using DakarRally.Core.DTO.FilterObjects;
using DakarRally.Models;
using System;
using System.Collections.Generic;

namespace DakarRally.Core.Services
{
    public interface IDakarRallyService
    {
        void AddRace(Race newRace);
        void AddVehicle(Guid raceId, Vehicle newVehicle);
        bool AnyRaceInProgress();
        Race GetRace(Guid raceId);
        IEnumerable<Race> GetRaces();
        Vehicle GetVehicleForRace(Guid raceId, Guid vehicleId);
        IEnumerable<Vehicle> GetVehiclesForRace(Guid raceId, FilterVehicleObject filterVehicleObject = null);
        IEnumerable<Vehicle> GetVehiclesForRace(Guid raceId, Enums.VehicleClass vClass);
        bool RaceExists(Guid raceId);
        void RemoveVehicle(Vehicle vehicle);
        bool Save();
        void UpdateVehicle(Vehicle vehicle);
    }
}