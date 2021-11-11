using DakarRally.Models;
using System.Threading;

namespace DakarRally.Core.Entities
{
    public interface IRaceQueue
    {
        void EnqueueRace(Race race);
        Race DequeueRace(CancellationToken cancellationToken);
    }
}
