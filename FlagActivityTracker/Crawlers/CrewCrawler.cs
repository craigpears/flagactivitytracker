using FlagActivityTracker.Database;
using FlagActivityTracker.Entities;
using FlagActivityTracker.Parsers;
using FlagActivityTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagActivityTracker.Crawlers
{
    public class CrewCrawler
    {
        private readonly FlagActivityTrackerDbContext _ctx;
        private readonly ICrewPageParser _crewPageParser;
        private readonly IPageScrapeService _pageScrapeService;

        public CrewCrawler(FlagActivityTrackerDbContext ctx,
            ICrewPageParser crewPageParser,
            IPageScrapeService pageScrapeService)
        {
            _ctx = ctx;
            _crewPageParser = crewPageParser;
            _pageScrapeService = pageScrapeService;
        }

        public void GeneratePageScrapeRequests()
        {
            var anHourAgo = DateTime.UtcNow.AddHours(-1);
            var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
            var tenMinutesAgo = DateTime.UtcNow.AddMinutes(-10);

            // TODO: scan blockading crews every 5 minutes
            // TODO: Change the tests that were based on 5 minutes for these
            var activelyJobbingCrews = _ctx.Crews.Where(x => x.JobbersLastSeen > anHourAgo && x.LastParsedDate < tenMinutesAgo).ToList();

            var crewsToRefresh = _ctx.Crews.Where(x =>
                   x.LastParsedDate == null || x.LastParsedDate < anHourAgo
            )
                .OrderByDescending(x => x.Voyages.Count)  // TODO: Add test + time limit the voyages?
                                                          // TODO: Prioritise large voyages over numerous ones
                .ToList();

            var crews = new List<Crew>();
            //crews.AddRange(activelyJobbingCrews); // TODO: Put this back in later, we just want to work out which are the crews with any activity for now
            crews.AddRange(crewsToRefresh);

            var pageScrapeRequests = crews.Select(x => new PageScrape
            {
                PageType = PageType.Crew,
                EntityId = x.CrewId,
                PuzzlePiratesId = x.PPCrewId.ToString(),
                EntityName = x.CrewName ?? "Unknown Crew"
            }).ToList();

            _pageScrapeService.QueuePageScrapeRequests(pageScrapeRequests);
        }

        public void ProcessPageScrapes()
        {
            var pageScrapesToProcess = _ctx.PageScrapes.Where(x => x.PageType == PageType.Crew && !x.Processed && x.DownloadedDate != null).ToList();
            foreach (var pageScrape in pageScrapesToProcess)
            {
                ProcessPageScrape(pageScrape);
            }
        }

        public void ProcessPageScrape(PageScrape pageScrape)
        {
            try
            {
                var crew = _ctx.Crews.Single(x => x.CrewId == pageScrape.EntityId);
                Console.WriteLine($"Processing Crew Page - {crew.CrewName} ({crew.PPCrewId})");
                var parsedCrewPage = _crewPageParser.ParsePage(pageScrape.DownloadedHtml);

                crew.CrewName = parsedCrewPage.CrewName;

                if (parsedCrewPage.JobbingPirates.Any())
                {
                    Console.WriteLine($"Found {parsedCrewPage.JobbingPirates.Count} jobbing pirates!!!");
                    crew.JobbersLastSeen = DateTime.UtcNow;
                }

                var newPirates = parsedCrewPage.JobbingPirates.Where(x => !_ctx.Pirates.Any(y => y.PirateName == x)).ToList();
                _ctx.Pirates.AddRange(newPirates.Select(x => new Pirate { PirateName = x }));
                crew.LastParsedDate = DateTime.UtcNow;

                _ctx.SaveChanges();

                // TODO: Add tests for jobbing activity
                var jobbingActivities = parsedCrewPage.JobbingPirates.Select(x => new JobbingActivity
                {
                    ActivityDate = DateTime.UtcNow,
                    Pirate = _ctx.Pirates.Single(y => y.PirateName == x),
                    CrewId = crew.CrewId
                });

                _ctx.JobbingActivities.AddRange(jobbingActivities);
                pageScrape.Processed = true;

                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing crew page scrape {pageScrape.PageScrapeId} - {ex.Message}");
            }
        }
    }
}
