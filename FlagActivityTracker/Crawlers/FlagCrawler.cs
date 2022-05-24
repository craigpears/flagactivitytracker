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
    public class FlagCrawler
    {
        private readonly FlagActivityTrackerDbContext _ctx;
        private readonly IFlagPageParser _flagPageParser;
        private readonly IPageScrapeService _pageScrapeService;

        public FlagCrawler(FlagActivityTrackerDbContext ctx,
            IFlagPageParser flagPageParser,
            IPageScrapeService pageScrapeService)
        {
            _ctx = ctx;
            _flagPageParser = flagPageParser;
            _pageScrapeService = pageScrapeService;
        }

        public void GeneratePageScrapeRequests()
        {
            var aDayAgo = DateTime.UtcNow.AddDays(-1);

            var flagsToRefresh = _ctx.Flags.Where(x => x.LastParsedDate == null || x.LastParsedDate < aDayAgo).ToList();

            var pageScrapeRequests = flagsToRefresh.Select(x => new PageScrape
            {
                PageType = PageType.Flag,
                EntityId = x.FlagId,
                PuzzlePiratesId = x.PPFlagId.ToString(),
                EntityName = x?.FlagName ?? "Unknown Flag"
            }).ToList();

            _pageScrapeService.QueuePageScrapeRequests(pageScrapeRequests);
        }

        public void ProcessPageScrapes()
        {
            var pageScrapesToProcess = _ctx.PageScrapes.Where(x => x.PageType == PageType.Flag && !x.Processed && x.DownloadedDate != null).ToList();
            foreach (var pageScrape in pageScrapesToProcess)
            {
                ProcessPageScrape(pageScrape);
            }
        }

        public void ProcessPageScrape(PageScrape pageScrape)
        {
            try
            {
                Console.WriteLine($"Processing flag page for {pageScrape.EntityName} ({pageScrape.PuzzlePiratesId})");
                var flag = _ctx.Flags.Single(x => x.FlagId == pageScrape.EntityId);
                var parsedFlagPage = _flagPageParser.ParsePage(pageScrape.DownloadedHtml);
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

                flag.LastParsedDate = DateTime.UtcNow;
                pageScrape.Processed = true;

                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing flag page for {pageScrape.EntityName} - {ex.Message}");
            }
        }
    }
}
