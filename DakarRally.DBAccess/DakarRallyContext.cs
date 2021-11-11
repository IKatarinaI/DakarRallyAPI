using DakarRally.Models;
using Microsoft.EntityFrameworkCore;

namespace DakarRally.DBAccess
{
    public class DakarRallyContext : DbContext
    {
        public DakarRallyContext(DbContextOptions<DakarRallyContext> options)
            : base(options)
        {

        }

        public DbSet<Race> Races { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
    }
}
