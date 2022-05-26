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
            Console.WriteLine("Generating crew page scrape requests");

            var anHourAgo = DateTime.UtcNow.AddHours(-1);
            var thirtyMinutesAgo = DateTime.UtcNow.AddMinutes(-30);
            var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
            var tenMinutesAgo = DateTime.UtcNow.AddMinutes(-10);

            // TODO: scan blockading crews every 5 minutes
            // TODO: Change the tests that were based on 5 minutes for these
            var activelyJobbingCrews = _ctx.Crews.Where(x => x.JobbersLastSeen > anHourAgo && x.LastParsedDate < tenMinutesAgo && x.DeletedDate == null).ToList();

            // TODO: Stop scanning crews that have been scanned several times and never job
            // TODO: Have a setting to set frequency per crew so you can tune which ones to watch
            // TODO: Scan crews that do large voyages even more often than those that just do small ones?

            // TODO: Scan flag pages and get jobbing numbers from there rather than crews to cut down on page requests?

            var crewsToRefresh = _ctx.Crews.Where(x =>
                   (x.LastParsedDate == null || x.LastParsedDate < thirtyMinutesAgo) && x.DeletedDate == null
                   && x.Voyages.Any()
            )
                .OrderByDescending(x => x.Voyages.Count)  // TODO: Add test + time limit the voyages?
                                                          // TODO: Prioritise large voyages over numerous ones
                .ToList();

            var crews = new List<Crew>();
            //crews.AddRange(activelyJobbingCrews); // TODO: Put this back in later, we just want to work out which are the crews with any activity for now
            crews.AddRange(crewsToRefresh);

            // If there aren't many, pick some random crews to scan
            var crewsCount = crews.Count; // TODO: Add tests for this
            var randomCrewsToTake = Math.Max(0, 8 - crewsCount);
            var randomCrewsToScan = _ctx.Crews.Where(x => x.DeletedDate == null).OrderBy(x => x.LastParsedDate).Take(randomCrewsToTake).ToList();
            crews.AddRange(randomCrewsToScan);

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

                if(parsedCrewPage.CrewDoesNotExist)
                {
                    crew.DeletedDate = pageScrape.DownloadedDate;
                }
                else
                {
                    crew.CrewName = parsedCrewPage.CrewName;

                    if (parsedCrewPage.JobbingPirates.Any())
                    {
                        Console.WriteLine($"Found {parsedCrewPage.JobbingPirates.Count} jobbing pirates!!!");
                        crew.JobbersLastSeen = DateTime.UtcNow;
                    }

                    var newPirates = parsedCrewPage.JobbingPirates.Where(x => !_ctx.Pirates.Any(y => y.PirateName == x)).ToList();
                    _ctx.Pirates.AddRange(newPirates.Select(x => new Pirate { PirateName = x }));
                    crew.LastParsedDate = (DateTime)pageScrape.DownloadedDate;

                    _ctx.SaveChanges();

                    if (parsedCrewPage.JobbingPirates.Contains("Balloffire"))
                        Console.WriteLine("Smoking hot gunner spotted!!!");

                    // TODO: Add tests for jobbing activity
                    var jobbingActivities = parsedCrewPage.JobbingPirates.Select(x => new JobbingActivity
                    {
                        ActivityDate = (DateTime)pageScrape.DownloadedDate,
                        Pirate = _ctx.Pirates.Single(y => y.PirateName == x),
                        CrewId = crew.CrewId
                    });

                    _ctx.JobbingActivities.AddRange(jobbingActivities);
                }

                pageScrape.Processed = true;

                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing crew page scrape {pageScrape.PageScrapeId} - {ex.Message}");
                pageScrape.ProcessingErrorMessage = ex.Message;
                pageScrape.Processed = true;
                _ctx.SaveChanges();
            }
        }
    }
}
