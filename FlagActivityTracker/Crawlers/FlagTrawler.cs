using FlagActivityTracker.Database;
using FlagActivityTracker.Entities;
using FlagActivityTracker.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagActivityTracker.Crawlers
{
    public class FlagTrawler
    {
        private readonly FlagActivityTrackerDbContext _ctx;
        private readonly IFlagPageParser _flagPageParser;

        public FlagTrawler(FlagActivityTrackerDbContext ctx,
            IFlagPageParser flagPageParser)
        {
            _ctx = ctx;
            _flagPageParser = flagPageParser;
        }

        public void TrawlFlags()
        {
            var aDayAgo = DateTime.UtcNow.AddDays(-1);
            var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);

            var flagsToRefresh = _ctx.Flags.Where(x =>
                      (x.LastParsedDate == null || x.LastParsedDate < aDayAgo)
                   && x.ErrorCount == 0
               ).ToList();
            var flagsToReprocess = _ctx.Flags.Where(x => x.ErrorCount > 0 && x.LastErrorDate < fiveMinutesAgo).ToList();
           
            var flagsToPopulate = new List<Flag>();
            flagsToPopulate.AddRange(flagsToRefresh);
            flagsToPopulate.AddRange(flagsToReprocess);

            foreach (var flag in flagsToPopulate)
            {
                Console.WriteLine($"Crawling Flag - {flag.FlagName} ({flag.PPFlagId})");
                var parsedFlagPage = _flagPageParser.DownloadFlagPage(flag.PPFlagId);
                if(parsedFlagPage != null)
                {
                    flag.FlagName = parsedFlagPage.FlagName;

                    var newCrews = parsedFlagPage.PuzzlePirateCrewIds.Where(x => !flag.Crews.Any(y => y.PPCrewId == x));
                    
                    foreach (var newCrew in newCrews)
                    {
                        var existingCrew = _ctx.Crews.SingleOrDefault(x => x.PPCrewId == newCrew);
                        // TODO: Add tests around crews changing flags
                        if (existingCrew != null)
                        {
                            existingCrew.Flag = flag;
                        }
                        else
                        {
                            _ctx.Crews.Add(new Crew
                            {
                                PPCrewId = newCrew,
                                Flag = flag
                            });
                        }
                    }

                    _ctx.SaveChanges();

                    flag.LastParsedDate = DateTime.UtcNow;
                    flag.ErrorCount = 0;
                }
                else
                {
                    flag.LastErrorDate = DateTime.UtcNow;
                    flag.ErrorCount++;
                }

                _ctx.SaveChanges();
            }

        }
    }
}
