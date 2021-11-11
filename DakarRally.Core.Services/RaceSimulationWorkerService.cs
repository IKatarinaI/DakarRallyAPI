using DakarRally.Core.Entities;
using DakarRally.DBAccess;
using DakarRally.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DakarRally.Core.Services
{
    public class RaceSimulationWorkerService : IHostedService
    {
        private IRaceQueue _startingRaceQueue;
        private IConfiguration _configuration;
        private string connectionString;
        private DbContextOptionsBuilder<DakarRallyContext> optionsBuilder;
        private CancellationTokenSource cts;

        public RaceSimulationWorkerService(
            IRaceQueue startingRaceQueue,
            IConfiguration configuration)
        {
            _startingRaceQueue = startingRaceQueue;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("databaseName");
            optionsBuilder = new DbContextOptionsBuilder<DakarRallyContext>();
            optionsBuilder.UseInMemoryDatabase(connectionString);
            cts = new CancellationTokenSource();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Thread worker = new Thread(() => Worker(cancellationToken));
            worker.Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cts.Cancel();
            return Task.CompletedTask;
        }

        private void Worker(CancellationToken cancellationToken)
        {
            // Waits until there is race that needs to be started. Loop runs until the application is shut down. 
            while (!cancellationToken.IsCancellationRequested && !cts.IsCancellationRequested)
            {
                Race raceToStart;
                try
                {
                    // Dequeue operation will be blocked until Race instance is enqueued in order for it to be started.
                    raceToStart = _startingRaceQueue.DequeueRace(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                using (DakarRallyContext dbContext = new DakarRallyContext(optionsBuilder.Options))
                {
                    raceToStart.Vehicles = dbContext.Vehicles
                        .Where(c => c.RaceId == raceToStart.Id).ToList<Vehicle>();
                }

                raceToStart.StartRace(cts.Token, UpdateVehicle, FinishRace);
            }
        }

        private void UpdateVehicle(Vehicle vehicleForUpdate)
        {
            using (DakarRallyContext dbContext = new DakarRallyContext(optionsBuilder.Options))
            {
                dbContext.Vehicles.Update(vehicleForUpdate);
                dbContext.SaveChanges();
            }
        }

        private void FinishRace(Race finishedRace)
        {
            using (DakarRallyContext dbContext = new DakarRallyContext(optionsBuilder.Options))
            {
                dbContext.Races.Update(finishedRace);
                dbContext.SaveChanges();
            }
        }
    }
}
