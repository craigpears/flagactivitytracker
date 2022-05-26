using FlagActivityTracker.Database;
using FlagActivityTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagActivityTracker
{
    public class VoyageProcessor
    {
        private readonly FlagActivityTrackerDbContext _ctx;

        public VoyageProcessor(FlagActivityTrackerDbContext ctx)
        {
            _ctx = ctx;
        }

        public void ProcessJobbingActivity()
        {
            // TODO: Add tests etc.
            Console.WriteLine("Processing voyages");
            var orphanedJobActivities = _ctx.JobbingActivities
                .Where(x => x.VoyageId == null)
                .OrderBy(x => x.ActivityDate)
                .ToList();

            foreach (var activity in orphanedJobActivities)
            {
                var matchingVoyage = _ctx.Voyages
                    .FirstOrDefault(x => x.CrewId == activity.CrewId
                                && x.StartTime.AddMinutes(-70) < activity.ActivityDate
                                && x.EndTime.AddMinutes(70) > activity.ActivityDate);

                if (matchingVoyage != null)
                {
                    activity.Voyage = matchingVoyage;
                    if (activity.ActivityDate < matchingVoyage.StartTime)
                    {
                        matchingVoyage.StartTime = activity.ActivityDate;
                    }

                    if (activity.ActivityDate > matchingVoyage.EndTime)
                    {
                        matchingVoyage.EndTime = activity.ActivityDate;
                    }
                }
                else
                {
                    Console.WriteLine("Adding new voyage!");
                    var voyage = new Voyage
                    {
                        CrewId = activity.CrewId,
                        StartTime = activity.ActivityDate,
                        EndTime = activity.ActivityDate
                    };

                    _ctx.SaveChanges();

                    activity.Voyage = voyage;
                }

                _ctx.SaveChanges();
            }
        }
    }
}
