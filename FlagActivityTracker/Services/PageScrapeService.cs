using FlagActivityTracker.Database;
using FlagActivityTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagActivityTracker.Services
{
    public interface IPageScrapeService
    {
        void DownloadPages();
        void QueuePageScrapeRequests(List<PageScrape> requests);
    }

    public class PageScrapeService : IPageScrapeService
    {
        private readonly FlagActivityTrackerDbContext _ctx;

        public PageScrapeService(FlagActivityTrackerDbContext ctx)
        {
            _ctx = ctx;
        }

        public void QueuePageScrapeRequests(List<PageScrape> requests)
        {
            requests = requests.Where(x =>
                !_ctx.PageScrapes.Any(
                    y =>
                           y.EntityId == x.EntityId
                        && y.PageType == x.PageType
                        && !y.Processed
                )
            ).ToList();

            requests.ForEach(r => Console.WriteLine($"Queuing {r.PageType} page scrape request for {r.EntityName} ({r.PuzzlePiratesId})"));

            _ctx.PageScrapes.AddRange(requests);
            _ctx.SaveChanges();
        }

        public void DownloadPages()
        {
            var requests = _ctx.PageScrapes.Where(x => x.DownloadedDate == null).ToList();
            using var httpClient = new HttpClient();

            Parallel.ForEach(requests, new ParallelOptions { MaxDegreeOfParallelism = 5 }, request =>
            {
                var context = new FlagActivityTrackerDbContext();
                context.Attach(request);

                Console.WriteLine($"Scraping {request.PageType} page for {request.EntityName} ({request.PuzzlePiratesId})");

                string pageUrl = request.PageType switch
                {
                    PageType.Pirate => $"https://emerald.puzzlepirates.com/yoweb/pirate.wm?classic=false&target={request.PuzzlePiratesId}",
                    PageType.Crew => $"https://emerald.puzzlepirates.com/yoweb/crew/info.wm?crewid={request.PuzzlePiratesId}&classic=false",
                    PageType.Flag => $"https://emerald.puzzlepirates.com/yoweb/flag/info.wm?flagid={request.PuzzlePiratesId}&classic=false"
                };

                request.PageUrl = pageUrl;
                var proxyUrl = $"http://api.scraperapi.com?api_key=03339515441ba3541a70111efe98de57&url={pageUrl}";

                try
                {
                    try
                    {
                        // Try without the proxy first to save credits
                        request.DownloadedHtml = httpClient.GetStringAsync(pageUrl).Result;
                    }
                    catch (Exception)
                    {
                        request.DownloadedHtml = httpClient.GetStringAsync(proxyUrl).Result;
                    }

                    request.DownloadedDate = DateTime.Now;
                    _ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception loading {pageUrl}");
                    request.Attempts++;
                    _ctx.SaveChanges();
                }
            });
        }
    }
}
