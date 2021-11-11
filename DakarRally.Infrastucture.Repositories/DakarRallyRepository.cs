using DakarRally.Core.DTO.FilterObjects;
using DakarRally.DBAccess;
using DakarRally.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static DakarRally.Models.Enums;

namespace DakarRally.Infrastructure.Repositories
{
    public class DakarRallyRepository : IDakarRallyRepository
    {
        private readonly DakarRallyContext _context;
        public DakarRallyRepository(DakarRallyContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddRace(Race newRace)
        {
            if (newRace == null)
            {
                throw new ArgumentNullException(nameof(newRace));
            }

            newRace.Id = Guid.NewGuid();

            foreach (var vehicle in newRace.Vehicles)
            {
                vehicle.Id = Guid.NewGuid();
            }

            _context.Races.Add(newRace);
        }

        public void AddVehicle(Guid raceId, Vehicle newVehicle)
        {
            if (raceId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(raceId));
            }

            if (newVehicle == null)
            {
                throw new ArgumentNullException(nameof(newVehicle));
            }
            newVehicle.RaceId = raceId;
            _context.Vehicles.Add(newVehicle);
        }

        public bool AnyRaceInProgress()
        {
            return _context.Races.Any(r => r.Status == RaceStatus.Running);
        }

        public Race GetRace(Guid raceId)
        {
            if (raceId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(raceId));
            }

            return _context.Races.FirstOrDefault(r => r.Id == raceId);
        }

        public IEnumerable<Race> GetRaces()
        {
            return _context.Races.ToList<Race>();
        }

        public Vehicle GetVehicleForRace(Guid raceId, Guid vehicleId)
        {
            if (raceId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(raceId));
            }

            if (vehicleId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(vehicleId));
            }

            return _context.Vehicles
              .Where(v => v.RaceId == raceId && v.Id == vehicleId).FirstOrDefault();
        }

        public IEnumerable<Vehicle> GetVehiclesForRace(Guid raceId, FilterVehicleObject filterVehicleObject = null)
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            if (raceId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(raceId));
            }

            vehicles = _context.Vehicles
                            .Where(v => v.RaceId == raceId).ToList<Vehicle>();

            if (filterVehicleObject == null || (string.IsNullOrWhiteSpace(filterVehicleObject.TeamName) && string.IsNullOrWhiteSpace(filterVehicleObject.ModelName)
                    && !filterVehicleObject.ManufacturingDate.HasValue && !filterVehicleObject.Status.HasValue && !filterVehicleObject.CoveredDistance.HasValue))
            {
                return vehicles;
            }

            vehicles.RemoveAll(v =>
                (string.IsNullOrWhiteSpace(filterVehicleObject.TeamName) ? false : v.TeamName != filterVehicleObject.TeamName)
                || (string.IsNullOrWhiteSpace(filterVehicleObject.ModelName) ? false : v.ModelName != filterVehicleObject.ModelName)
                || (!filterVehicleObject.ManufacturingDate.HasValue ? false : v.ManufacturingDate != filterVehicleObject.ManufacturingDate)
                || (!filterVehicleObject.Status.HasValue ? false : v.Status != filterVehicleObject.Status)
                || (!filterVehicleObject.CoveredDistance.HasValue ? false : v.CoveredDistance != filterVehicleObject.CoveredDistance));

            return vehicles;
        }

        public IEnumerable<Vehicle> GetVehiclesForRace(Guid raceId, VehicleClass vClass)
        {
            if (raceId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(raceId));
            }

            return _context.Vehicles
                        .Where(v => v.RaceId == raceId && v.Class == vClass).ToList<Vehicle>();
        }

        public bool RaceExists(Guid raceId)
        {
            if (raceId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(raceId));
            }

            return _context.Races.Any(r => r.Id == raceId);
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            _context.SaveChanges();
        }
    }
}
