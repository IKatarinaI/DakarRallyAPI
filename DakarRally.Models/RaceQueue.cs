using DakarRally.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Threading;

namespace DakarRally.Core.Entities
{
    public class RaceQueue : IRaceQueue
    {
        private BlockingCollection<Race> queuedRaces;

        public RaceQueue(IConfiguration configuration)
        {
            queuedRaces = new BlockingCollection<Race>();
        }

        public Race DequeueRace(CancellationToken cancellationToken)
        {
            return queuedRaces.Take(cancellationToken);
        }

        public void EnqueueRace(Race race)
        {
            queuedRaces.Add(race);
        }
    }
}
