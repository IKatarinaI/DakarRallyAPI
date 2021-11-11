using System.Threading;

namespace DakarRally.Core.Entities
{
    public class DrivingParameters
    {
        public object LockObj { get; set; }

        // Event used when vehicle is ready to begin race
        public CountdownEvent Ready { get; set; }

        // Event used to stop vehicle in case it got to finish or suffered heavy mulfunction
        public AutoResetEvent RaceIsFinished { get; set; }

        // Used to notify vehicle about application shuting down
        public CancellationToken CancellationToken { get; set; }
    }
}
