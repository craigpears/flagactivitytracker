using FlagActivityTracker.Crawlers;
using FlagActivityTracker.Database;
using FlagActivityTracker.Entities;
using FlagActivityTracker.Models;
using FlagActivityTracker.Parsers;
using FlagActivityTracker.Services;
using Moq;
using Xunit;

namespace FlagActivityTracker.Tests.Crawlers
{
    public class PirateCrawlerTests
    {
        protected FlagActivityTrackerDbContext _ctx;
        protected PirateCrawler _crawler;
        protected Mock<IPiratePageParser> _parser;
        protected Mock<IPageScrapeService> _pageScrapeService;

        public PirateCrawlerTests()
        {            
            _ctx = new FlagActivityTrackerDbContext(useInMemoryDatabase: true);
            _parser = new Mock<IPiratePageParser>();
            _pageScrapeService = new Mock<IPageScrapeService>();

            _crawler = new PirateCrawler(_ctx, _parser.Object, _pageScrapeService.Object);

            _ctx.Pirates.Add(new Pirate { PirateName = "Bob" });
            _parser.Setup(x => x.ParsePage(It.IsAny<string>())).Returns(new ParsedPiratePage
            {
                PirateName = "Bob",
                PPCrewId = 123,
                PPFlagId = 456
            });

            _pageScrapeService.Setup(x => x.QueuePageScrapeRequests(It.IsAny<List<PageScrape>>())).Callback((List<PageScrape> pageScrapes) =>
            {
                pageScrapes.ForEach(p => p.DownloadedDate = DateTime.Now);
                _ctx.PageScrapes.AddRange(pageScrapes);
                _ctx.SaveChanges();
            });

            _ctx.SaveChanges();
        }

        [Fact]
        public void Should_Create_And_Link_Missing_Crews()
        {
            _crawler.GeneratePageScrapeRequests();
            _crawler.ProcessPageScrapes();

            Assert.Single(_ctx.Pirates);
            Assert.Single(_ctx.Flags);
            Assert.Single(_ctx.Crews);

            var pirate = _ctx.Pirates.Single();
            Assert.Equal("Bob", pirate.PirateName);
            Assert.Equal(123, pirate?.Crew?.PPCrewId);
            Assert.Equal(456, pirate?.Crew?.Flag?.PPFlagId);
            Assert.NotNull(pirate?.LastParsedDate);

            var pageScrape = _ctx.PageScrapes.Single();
            Assert.True(pageScrape.Processed);
        }

        [Fact]
        public void Should_Only_Populate_Unprocessed_Pirates()
        {
            _ctx.Pirates.Single().LastParsedDate = DateTime.MinValue;
            _ctx.SaveChanges();

            _crawler.GeneratePageScrapeRequests();
            _crawler.ProcessPageScrapes();

            Assert.Single(_ctx.Pirates);
            Assert.Empty(_ctx.Flags);
            Assert.Empty(_ctx.Crews);

            var pirate = _ctx.Pirates.Single();
            Assert.Equal("Bob", pirate.PirateName);
            Assert.Null(pirate.Crew);
        }

        [Fact]
        public void Should_Handle_Pirates_Without_Flags()
        {
            _parser.Setup(x => x.ParsePage(It.IsAny<string>())).Returns(new ParsedPiratePage
            {
                PirateName = "Bob",
                PPCrewId = 123,
                PPFlagId = null
            });

            _crawler.GeneratePageScrapeRequests();
            _crawler.ProcessPageScrapes();

            Assert.Single(_ctx.Pirates);
            Assert.Empty(_ctx.Flags);
            Assert.Single(_ctx.Crews);

            var pirate = _ctx.Pirates.Single();
            Assert.Equal("Bob", pirate.PirateName);
            Assert.Equal(123, pirate?.Crew?.PPCrewId);
            Assert.Null(pirate?.Crew?.Flag);
            Assert.NotNull(pirate?.LastParsedDate);
        }
    }
}