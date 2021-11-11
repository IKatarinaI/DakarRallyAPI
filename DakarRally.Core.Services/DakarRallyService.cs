using DakarRally.Core.DTO.FilterObjects;
using DakarRally.Infrastructure.Repositories;
using DakarRally.Models;
using System;
using System.Collections.Generic;
using static DakarRally.Models.Enums;

namespace DakarRally.Core.Services
{
    public class DakarRallyService : IDakarRallyService
    {
        private readonly IDakarRallyRepository _repository;
        public DakarRallyService(IDakarRallyRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Race> GetRaces()
        {
            return _repository.GetRaces();
        }

        public Race GetRace(Guid raceId)
        {
            return _repository.GetRace(raceId);
        }

        public bool RaceExists(Guid raceId)
        {
            return _repository.RaceExists(raceId);
        }

        public void AddRace(Race newRace)
        {
            _repository.AddRace(newRace);
        }

        public bool AnyRaceInProgress()
        {
            return _repository.AnyRaceInProgress();
        }

        public IEnumerable<Vehicle> GetVehiclesForRace(Guid raceId, FilterVehicleObject filterVehicleObject = null)
        {
            return _repository.GetVehiclesForRace(raceId, filterVehicleObject);
        }

        public IEnumerable<Vehicle> GetVehiclesForRace(Guid raceId, VehicleClass vClass)
        {
            return _repository.GetVehiclesForRace(raceId, vClass);
        }

        public Vehicle GetVehicleForRace(Guid raceId, Guid vehicleId)
        {
            return _repository.GetVehicleForRace(raceId, vehicleId);
        }

        public void AddVehicle(Guid raceId, Vehicle newVehicle)
        {
            _repository.AddVehicle(raceId, newVehicle);
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            _repository.RemoveVehicle(vehicle);
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            _repository.UpdateVehicle(vehicle);
        }

        public bool Save()
        {
            return _repository.Save();
        }
    }
}
