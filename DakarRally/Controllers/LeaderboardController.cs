using AutoMapper;
using DakarRally.API.Validators;
using DakarRally.Core.DTO.ReadObjects;
using DakarRally.Core.Services;
using DakarRally.DTO.ReadObjects;
using DakarRally.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using static DakarRally.Models.Enums;

namespace DakarRally.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly IDakarRallyService dakarRallyService;
        private readonly IMapper mapper;

        public LeaderboardController(IDakarRallyService dakarRallyService, IMapper mapper)
        {
            this.dakarRallyService = dakarRallyService ??
                throw new ArgumentNullException(nameof(dakarRallyService));
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        public ActionResult<IEnumerable<ReadVehicleRankObject>> GetLeaderboard(Guid raceId)
        {
            var raceFromRepo = dakarRallyService.GetRace(raceId);

            List<ReadVehicleRankObject> leaderboard = new List<ReadVehicleRankObject>();

            if (raceFromRepo == null)
            {
                return NotFound();
            }

            var leaderboardWithRaceStatus = mapper.Map<LeaderboardValidator>(raceFromRepo);

            if (!TryValidateModel(leaderboardWithRaceStatus))
            {
                return ValidationProblem(ModelState);
            }

            List<Vehicle> vehiclesFromRace = new List<Vehicle>();

            vehiclesFromRace = dakarRallyService.GetVehiclesForRace(raceId).ToList<Vehicle>();

            List<ReadVehicleObject> vehiclesForDisplay = (List<ReadVehicleObject>)mapper.Map<IEnumerable<ReadVehicleObject>>(vehiclesFromRace).ToList<ReadVehicleObject>();

            vehiclesForDisplay.Sort();

            for (int i = 0; i < vehiclesForDisplay.Count; i++)
            {
                leaderboard.Add(new ReadVehicleRankObject() { Rank = i + 1, Vehicle = vehiclesForDisplay[i] });
            }

            return Ok(leaderboard);
        }

        [HttpGet("{raceId}")]
        public ActionResult<IEnumerable<ReadVehicleRankObject>> GetLeaderboard(Guid raceId, [FromQuery] string vClass)
        {
            var raceFromRepo = dakarRallyService.GetRace(raceId);
            List<ReadVehicleRankObject> leaderboard = new List<ReadVehicleRankObject>();

            if (raceFromRepo == null)
            {
                return NotFound();
            }

            var leaderboardWithRaceStatus = mapper.Map<LeaderboardValidator>(raceFromRepo);

            if (!TryValidateModel(leaderboardWithRaceStatus))
            {
                return ValidationProblem(ModelState);
            }

            List<Vehicle> vehiclesFromRace = new List<Vehicle>();

            VehicleClass vehicleClass;
                if (Enum.TryParse<VehicleClass>(vClass, out vehicleClass))
                {
                    vehiclesFromRace = dakarRallyService.GetVehiclesForRace(raceId, vehicleClass).ToList<Vehicle>();
                }
                else
                {
                    return BadRequest();
                }

            List<ReadVehicleObject> vehiclesForDisplay = (List<ReadVehicleObject>)mapper.Map<IEnumerable<ReadVehicleObject>>(vehiclesFromRace).ToList<ReadVehicleObject>();

            vehiclesForDisplay.Sort();

            for (int i = 0; i < vehiclesForDisplay.Count; i++)
            {
                leaderboard.Add(new ReadVehicleRankObject() { Rank = i + 1, Vehicle = vehiclesForDisplay[i] });
            }

            return Ok(leaderboard);
        }
    }
}
