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
    public class PirateCrawler
    {
        private readonly FlagActivityTrackerDbContext _ctx;
        private readonly IPiratePageParser _piratePageParser;

        public PirateCrawler(FlagActivityTrackerDbContext ctx,
            IPiratePageParser piratePageParser)
        {
            _ctx = ctx;
            _piratePageParser = piratePageParser;
        }

        public void PopulatePirates()
        {
            var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
            var piratesToPopulate = _ctx.Pirates
                .Where(x => x.LastParsedDate == null)
                .Where(x => x.LastErrorDate == null || x.LastErrorDate < fiveMinutesAgo)
                .ToList();

            foreach (var pirate in piratesToPopulate)
            {
                Console.WriteLine($"Crawling Pirate - {pirate.PirateName}");
                var parsedPiratePage = _piratePageParser.DownloadPiratePage(pirate.PirateName);
                if(parsedPiratePage != null)
                {
                    if (parsedPiratePage.PPCrewId != null)
                    {
                        var crew = _ctx.Crews.SingleOrDefault(x => x.PPCrewId == parsedPiratePage.PPCrewId);
                        Flag? flag = null;

                        if(parsedPiratePage.PPFlagId != null) 
                        {
                            flag = _ctx.Flags.SingleOrDefault(x => x.PPFlagId == parsedPiratePage.PPFlagId);

                            if (flag == null)
                            {
                                flag = new Flag { PPFlagId = (int)parsedPiratePage.PPFlagId };
                                _ctx.Flags.Add(flag);
                                _ctx.SaveChanges();
                            }
                        }

                        if (crew == null)
                        {
                            crew = new Crew { PPCrewId = (int)parsedPiratePage.PPCrewId, FlagId = flag?.FlagId };
                            _ctx.Crews.Add(crew);
                            _ctx.SaveChanges();
                        }

                        pirate.CrewId = crew.CrewId;
                    }

                    pirate.LastParsedDate = DateTime.UtcNow;
                    pirate.ErrorCount = 0;
                }
                else
                {
                    pirate.LastErrorDate = DateTime.UtcNow;
                    pirate.ErrorCount++;
                }

                _ctx.SaveChanges();
            }

        }
    }
}
