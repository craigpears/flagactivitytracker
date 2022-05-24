using FlagActivityTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FlagActivityTracker.Database
{
    public class FlagActivityTrackerDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        private readonly bool _useInMemoryDatabase;

        public FlagActivityTrackerDbContext()
        {
        }

        public FlagActivityTrackerDbContext(bool useInMemoryDatabase)
        {
            _useInMemoryDatabase = useInMemoryDatabase;
        }

        public DbSet<Pirate> Pirates { get; set; }
        public DbSet<Flag> Flags { get; set; }
        public DbSet<Crew> Crews { get; set; }
        public DbSet<JobbingActivity> JobbingActivities { get; set; }
        public DbSet<Voyage> Voyages { get; set; }
        public DbSet<PageScrape> PageScrapes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if(_useInMemoryDatabase)
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }
            else
            {
                options.UseSqlServer("Data Source=(local); Initial Catalog=FlagActivityTracker; Trusted_Connection=true");
            }
        }
    }
}
