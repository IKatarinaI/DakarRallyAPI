using System;
using System.Collections.Generic;
using System.Threading;
using static DakarRally.Models.Enums;

namespace DakarRally.Core.Entities
{
    public class DrivingSimulation
    {
        protected VehicleType? VehicleType;
        protected int MaxSpeed;
        protected int LightMalfunction;
        protected int HeavyMalfunction;

        protected bool MalfunctionHappend(MalfunctionType malfunctionType)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            HashSet<int> generatedNumbers = new HashSet<int>();
            int malfunctionProb = malfunctionType == MalfunctionType.Heavy ? HeavyMalfunction : LightMalfunction;

            for (int i = 0; i < malfunctionProb;)
            {
                if (generatedNumbers.Add(rnd.Next(1, 101)))
                {
                    i++;
                }
            }

            int probability = rnd.Next(1, 101);

            return !generatedNumbers.Add(probability);
        }
        internal UpdateCoveredDistanceDelegate UpdateCoveredDistance;
        internal ReportMalfunctionDelegate ReportMalfunction;

        public virtual void SetType(VehicleType? _type) { }
        public virtual void Repair() { }

        public delegate void UpdateCoveredDistanceDelegate(int vehicleSpeed);

        public delegate void ReportMalfunctionDelegate(MalfunctionType malfunctionType);

        public void Drive(DrivingParameters drivingParameters)
        {
            lock (drivingParameters.LockObj)
            {
                drivingParameters.Ready.Signal();
                Monitor.Wait(drivingParameters.LockObj);
            }

            while (true)
            {
                // Wait for either race to be done, application to shut down or beggining of the new cycle of simulation
                var waitResult = WaitHandle.WaitAny(new WaitHandle[] { drivingParameters.RaceIsFinished, drivingParameters.CancellationToken.WaitHandle }, 1000);
                if (waitResult != WaitHandle.WaitTimeout)
                {
                    // In case when there is no need for new cycle, loop will be broken.
                    break;
                }
                UpdateCoveredDistance?.Invoke(MaxSpeed);

                // There is a chance for race to be done after covered distance is updated and is greater or equal to race length.
                if (drivingParameters.RaceIsFinished.WaitOne(0))
                {
                    break;
                }

                if (MalfunctionHappend(MalfunctionType.Heavy))
                {
                    // If heavy malfunction happend, it means race is done for this vehicle.
                    ReportMalfunction?.Invoke(MalfunctionType.Heavy);
                    break;
                }

                if (MalfunctionHappend(MalfunctionType.Light))
                {
                    // In case of light malfunction, next cycle will start after predefined repair function is over.
                    ReportMalfunction?.Invoke(MalfunctionType.Light);
                    Repair();
                }
            }
        }
    }
}
