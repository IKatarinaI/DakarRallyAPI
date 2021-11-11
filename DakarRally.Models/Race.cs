using System;
using System.Collections.Generic;
using System.Threading;
using static DakarRally.Models.Enums;

namespace DakarRally.Models
{
    public class Race
    {
        // Delegate used to signal to the worker that simulation is done
        private Action<Race> RaceIsFinished { get; set; }

        // Used to wait for all vehilces to be ready for start, before start is triggered
        private CountdownEvent VehiclesAreReady;

        public Guid Id { get; set; }
        public int Year { get; set; }
        public RaceStatus Status { get; set; } = RaceStatus.Pending;
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        // Starts the race simulation
        public void StartRace(CancellationToken cancellationToken, Action<Vehicle> UpdateVehicle, Action<Race> RaceFinished)
        {
            this.RaceIsFinished = RaceFinished;
            object semaphore = new object();
            CountdownEvent vehiclesReady = new CountdownEvent(Vehicles.Count);
            VehiclesAreReady = new CountdownEvent(Vehicles.Count);

            foreach (var vehicle in Vehicles)
            {
                vehicle.StartEngines(semaphore, vehiclesReady, cancellationToken, UpdateVehicle, FinishRace);
            }

            vehiclesReady.Wait();

            lock (semaphore)
            {
                // After all vehicles reaches the same point, signal them to begin the race.
                Monitor.PulseAll(semaphore);
            }
        }

        // Used to monitor the state of the race.
        private void FinishRace()
        {
            VehiclesAreReady.Signal();
            if (VehiclesAreReady.IsSet)
            {
                // In case when all of the vehicles are either finished or have suffered heavy malfunction, simulation is done.
                Status = RaceStatus.Finished;
                RaceIsFinished?.Invoke(this);
            }
        }
    }
}
