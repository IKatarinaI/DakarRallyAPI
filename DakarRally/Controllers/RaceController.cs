using AutoMapper;
using DakarRally.API.Validators;
using DakarRally.Core.DTO.ReadObjects;
using DakarRally.Core.Entities;
using DakarRally.Core.Services;
using DakarRally.DTO.CreateObjects;
using DakarRally.DTO.ReadObjects;
using DakarRally.DTO.UpdateObjects;
using DakarRally.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DakarRally.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RaceController : ControllerBase
    {
        private readonly IDakarRallyService dakarRallyService;
        private readonly IMapper mapper;
        private IRaceQueue worker;
        private static object lockObj = new object();

        public RaceController(IDakarRallyService dakarRallyService, IMapper mapper, IRaceQueue worker)
        {
            this.dakarRallyService = dakarRallyService ??
                throw new ArgumentNullException(nameof(dakarRallyService));
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            this.worker = worker ??
                throw new ArgumentNullException(nameof(worker));
        }

        [HttpPost()]
        public ActionResult<ReadRaceObject> CreateRace(CreateRaceObject newRace)
        {
            var raceEntity = mapper.Map<Race>(newRace);
            dakarRallyService.AddRace(raceEntity);
            dakarRallyService.Save();

            var raceForDisplay = mapper.Map<ReadRaceObject>(raceEntity);

            return CreatedAtRoute("GetRace",
                new { raceId = raceForDisplay.Id },
                raceForDisplay);
        }

        [HttpGet("{raceId}", Name = "GetRace")]
        public IActionResult GetRace(Guid raceId)
        {
            var raceFromRepo = dakarRallyService.GetRace(raceId);

            if (raceFromRepo == null)
            {
                return NotFound();
            }

            raceFromRepo.Vehicles = dakarRallyService.GetVehiclesForRace(raceId).ToList();

            return Ok(mapper.Map<ReadRaceObject>(raceFromRepo));
        }

        [HttpGet()]
        public ActionResult<IEnumerable<ReadRaceObject>> GetRaces()
        {
            var racesFromRepo = dakarRallyService.GetRaces();
            foreach (var race in racesFromRepo)
            {
                race.Vehicles = dakarRallyService.GetVehiclesForRace(race.Id).ToList();
            }

            var races = mapper.Map<IEnumerable<ReadRaceObject>>(racesFromRepo);

            return Ok(races);
        }

        [HttpGet("status/{raceId}", Name = "GetRaceStatus")]
        public IActionResult GetRaceStatus(Guid raceId)
        {
            var raceFromRepo = dakarRallyService.GetRace(raceId);

            if (raceFromRepo == null)
            {
                return NotFound();
            }

            ReadRaceStatusObject raceStatusForDisplay = new ReadRaceStatusObject() { Status = raceFromRepo.Status.ToString() };

            raceFromRepo.Vehicles = dakarRallyService.GetVehiclesForRace(raceId).ToList<Vehicle>();

            var groupByStatus = raceFromRepo.Vehicles.GroupBy(v => v.Status).ToHashSet();
            var groupByClass = raceFromRepo.Vehicles.GroupBy(v => v.Class).ToHashSet();

            foreach (var item in groupByStatus)
            {
                raceStatusForDisplay.NumberOfVehiclesByStatus[item.Key.ToString()] = item.Count();
            }

            foreach (var item in groupByClass)
            {
                raceStatusForDisplay.NumberOfVehiclesByClass[item.Key.ToString()] = item.Count();
            }

            return Ok(raceStatusForDisplay);
        }

        [HttpPatch("{raceId}")]
        public ActionResult StartRace(Guid raceId, UpdateRaceObject updateRaceObject)
        {
            var raceFromRepo = dakarRallyService.GetRace(raceId);

            if (raceFromRepo == null)
            {
                return NotFound();
            }

            var raceToPatch = mapper.Map<UpdateRaceObject>(updateRaceObject);

            var raceValidationHelper = mapper.Map<RaceValidator>(raceToPatch);
            raceValidationHelper.OldStatus = raceFromRepo.Status;

            // Validation to ensure status transition is valid.
            if (!TryValidateModel(raceValidationHelper))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(raceToPatch, raceFromRepo);

            // Critical section used to ensure that only one race can be started at once.
            lock (lockObj)
            {
                if (dakarRallyService.AnyRaceInProgress())
                {
                    return BadRequest();
                }

                dakarRallyService.Save();
                worker.EnqueueRace(raceFromRepo);
            }

            return NoContent();
        }
    }
}
