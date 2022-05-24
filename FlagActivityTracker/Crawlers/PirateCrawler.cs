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
    public class PirateCrawler
    {
        private readonly FlagActivityTrackerDbContext _ctx;
        private readonly IPiratePageParser _piratePageParser;
        private readonly IPageScrapeService _pageScrapeService;

        public PirateCrawler(FlagActivityTrackerDbContext ctx,
            IPiratePageParser piratePageParser,
            IPageScrapeService pageScrapeService)
        {
            _ctx = ctx;
            _piratePageParser = piratePageParser;
            _pageScrapeService = pageScrapeService;
        }

        public void GeneratePageScrapeRequests()
        {
            Console.WriteLine("Generating pirate page scrape requests");

            var piratesToPopulate = _ctx.Pirates
                .Where(x => x.LastParsedDate == null && x.DeletedDate == null)
                .ToList();

            var pageScrapeRequests = piratesToPopulate.Select(x => new PageScrape
            {
                PageType = PageType.Pirate,
                EntityId = x.PirateId,
                PuzzlePiratesId = x.PirateName.ToString(),
                EntityName = x.PirateName
            }).ToList();

            _pageScrapeService.QueuePageScrapeRequests(pageScrapeRequests);
        }

        public void ProcessPageScrapes()
        {
            var pageScrapesToProcess = _ctx.PageScrapes.Where(x => x.PageType == PageType.Pirate && !x.Processed && x.DownloadedDate != null).ToList();
            foreach (var pageScrape in pageScrapesToProcess)
            {
                ProcessPageScrape(pageScrape);
            }
        }

        public void ProcessPageScrape(PageScrape pageScrape)
        {
            try
            {
                Console.WriteLine($"Processing pirate page for {pageScrape.EntityName}");

                var pirate = _ctx.Pirates.Single(x => x.PirateId == pageScrape.EntityId);
                var parsedPiratePage = _piratePageParser.ParsePage(pageScrape.DownloadedHtml);

                if (parsedPiratePage.PPCrewId != null)
                {
                    var crew = _ctx.Crews.SingleOrDefault(x => x.PPCrewId == parsedPiratePage.PPCrewId);
                    Flag? flag = null;

                    if (parsedPiratePage.PPFlagId != null)
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

                pirate.LastParsedDate = (DateTime)pageScrape.DownloadedDate;
                pageScrape.Processed = true;

                _ctx.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing pirate page for {pageScrape.EntityName} - {ex.Message}");
                pageScrape.ProcessingErrorMessage = ex.Message;
                pageScrape.Processed = true;
                _ctx.SaveChanges();
            }
        }

    }
}
