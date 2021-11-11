using AutoMapper;
using DakarRally.API.Validators;
using DakarRally.Core.DTO.FilterObjects;
using DakarRally.Core.DTO.ReadObjects;
using DakarRally.Core.Services;
using DakarRally.DTO.CreateObjects;
using DakarRally.DTO.ReadObjects;
using DakarRally.DTO.UpdateObjects;
using DakarRally.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DakarRally.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IDakarRallyService dakarRallyService;
        private readonly IMapper mapper;

        public VehicleController(IDakarRallyService dakarRallyService, IMapper mapper)
        {
            this.dakarRallyService = dakarRallyService ??
                throw new ArgumentNullException(nameof(dakarRallyService));
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost()]
        public ActionResult<ReadVehicleObject> CreateVehicleForRace(Guid raceId, CreateVehicleObject newVehicle)
        {
            var raceFromRepo = dakarRallyService.GetRace(raceId);

            if (raceFromRepo == null)
            {
                return NotFound();
            }

            var vehicleEntity = mapper.Map<Vehicle>(newVehicle);
            vehicleEntity.Race = raceFromRepo;
            var vehicleValidationHelper = mapper.Map<VehicleValidator>(vehicleEntity);

            // Check to see if race with provided identifier is in valid state (pending). Vehicle can't be added to race that is in progress or is finished.
            if (!TryValidateModel(vehicleValidationHelper))
            {
                return ValidationProblem(ModelState);
            }

            dakarRallyService.AddVehicle(raceId, vehicleEntity);
            dakarRallyService.Save();

            var vehicleForDisplay = mapper.Map<ReadVehicleObject>(vehicleEntity);

            return CreatedAtRoute("GetVehicleForRace",
                new { raceId = raceId, vehicleId = vehicleForDisplay.Id }, vehicleForDisplay);
        }

        [HttpGet("{vehicleId}", Name = "GetVehicleForRace")]
        public ActionResult<ReadVehicleObject> GetVehicleForRace(Guid raceId, Guid vehicleId)
        {
            if (!dakarRallyService.RaceExists(raceId))
            {
                return NotFound();
            }

            var vehicleForRaceFromRepo = dakarRallyService.GetVehicleForRace(raceId, vehicleId);

            if (vehicleForRaceFromRepo == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ReadVehicleObject>(vehicleForRaceFromRepo));
        }

        [HttpGet()]
        public ActionResult<IEnumerable<ReadVehicleObject>> GetFilteredVehicles(Guid raceId, [FromQuery] FilterVehicleObject filterVehicleObject)
        {
            if (!dakarRallyService.RaceExists(raceId))
            {
                return NotFound();
            }

            var vehiclesForRaceFromRepo = dakarRallyService.GetVehiclesForRace(raceId, filterVehicleObject);
            return Ok(mapper.Map<IEnumerable<ReadVehicleObject>>(vehiclesForRaceFromRepo));
        }

        [HttpDelete()]
        public ActionResult RemoveVehicleFromRace(Guid raceId, Guid vehicleId)
        {
            var raceFromRepo = dakarRallyService.GetRace(raceId);
            if (raceFromRepo == null)
            {
                return NotFound();
            }

            var vehicleFromRepo = dakarRallyService.GetVehicleForRace(raceId, vehicleId);
            if (vehicleFromRepo == null)
            {
                return NotFound();
            }

            vehicleFromRepo.Race = raceFromRepo;

            var vehicleValidationHelper = mapper.Map<VehicleValidator>(vehicleFromRepo);

            // Vehicle can't be removed from race once it is started or has been finished.
            if (!TryValidateModel(vehicleValidationHelper))
            {
                return ValidationProblem(ModelState);
            }

            dakarRallyService.RemoveVehicle(vehicleFromRepo);
            dakarRallyService.Save();

            return NoContent();
        }

        [HttpGet("statistics/{vehicleId}", Name = "GetVehiclesStatistics")]
        public IActionResult GetVehiclesStatistics(Guid raceId, Guid vehicleId)
        {
            if (!dakarRallyService.RaceExists(raceId))
            {
                return NotFound();
            }

            var vehicleFromRepo = dakarRallyService.GetVehicleForRace(raceId, vehicleId);

            if (vehicleFromRepo == null)
            {
                return NotFound();
            }

            var vehicleStatisticsForDisplay = mapper.Map<ReadVehicleStatisticsObject>(vehicleFromRepo);

            return Ok(vehicleStatisticsForDisplay);
        }

        [HttpPatch("{vehicleId}")]
        public ActionResult ModifyVehicle(Guid raceId, Guid vehicleId, UpdateVehicleObject updateVehicleObject)
        {
            var raceFromRepo = dakarRallyService.GetRace(raceId);

            if (raceFromRepo == null)
            {
                return NotFound();
            }

            var vehicleFromRepo = dakarRallyService.GetVehicleForRace(raceId, vehicleId);

            if (vehicleFromRepo == null)
            {
                return NotFound();
            }

            //vehicleFromRepo.Race = raceFromRepo;

            var vehicleToPatch = mapper.Map<UpdateVehicleObject>(updateVehicleObject);

            //updateVehicleObject.ApplyTo(vehicleToPatch, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);

            var vehicleValidationHelper = mapper.Map<VehicleValidator>(vehicleToPatch);
            vehicleValidationHelper.RaceYear = raceFromRepo.Year;
            vehicleValidationHelper.RaceStatus = raceFromRepo.Status;

            if (!TryValidateModel(vehicleValidationHelper))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(vehicleToPatch, vehicleFromRepo);

            dakarRallyService.UpdateVehicle(vehicleFromRepo);
            dakarRallyService.Save();

            return NoContent();
        }
    }
}
