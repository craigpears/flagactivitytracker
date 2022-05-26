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
    public class CrewTrawlerTests
    {
        protected FlagActivityTrackerDbContext _ctx;
        protected CrewCrawler _crawler;
        protected Mock<ICrewPageParser> _parser;
        protected Mock<IPageScrapeService> _pageScrapeService;

        public CrewTrawlerTests()
        {            
            _ctx = new FlagActivityTrackerDbContext(useInMemoryDatabase: true);
            _parser = new Mock<ICrewPageParser>();
            _pageScrapeService = new Mock<IPageScrapeService>();

            _crawler = new CrewCrawler(_ctx, _parser.Object, _pageScrapeService.Object);

            _ctx.Crews.Add(new Crew { PPCrewId = 123 });
            _parser.Setup(x => x.ParsePage(It.IsAny<string>())).Returns(new ParsedCrewPage()
            {
                CrewName = "Bob's Pirates"
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
        public void Should_Populate_Crew_Name()
        {
            _crawler.GeneratePageScrapeRequests();
            _crawler.ProcessPageScrapes();

            var crew = _ctx.Crews.Single();
            Assert.Equal("Bob's Pirates", crew.CrewName);
            Assert.NotNull(crew.LastParsedDate);

            var pageScrape = _ctx.PageScrapes.Single();
            Assert.True(pageScrape.Processed);
        }

        [Fact]
        public void Should_Not_Trawl_Crews_More_Than_Once_Every_Thirty_Minutes()
        {
            _ctx.Crews.Single().LastParsedDate = DateTime.UtcNow.AddMinutes(-15);
            _ctx.SaveChanges();

            _crawler.GeneratePageScrapeRequests();
            _crawler.ProcessPageScrapes();

            var crew = _ctx.Crews.Single();
            Assert.Null(crew.CrewName);
        }

        /*
        [Fact]
        public void Should_Trawl_Crews_More_Often_If_They_Had_Jobbers_Recently()
        {
            _ctx.Crews.Single().LastParsedDate = DateTime.UtcNow.AddMinutes(-30);
            _ctx.Crews.Single().JobbersLastSeen = DateTime.UtcNow.AddMinutes(-6);
            _ctx.SaveChanges();

            _crawler.GeneratePageScrapeRequests();
            _crawler.ProcessPageScrapes();

            var crew = _ctx.Crews.Single();
            Assert.Equal("Bob's Pirates", crew.CrewName);
        }
        */

        [Fact]
        public void Should_Set_Jobbers_Last_Seen_When_There_Are_Jobbers()
        {
            _parser.Setup(x => x.ParsePage(It.IsAny<string>())).Returns(new ParsedCrewPage()
            {
                CrewName = "Bob's Pirates",
                JobbingPirates = new List<string> { "Bob" }
            });

            _crawler.GeneratePageScrapeRequests();
            _crawler.ProcessPageScrapes();

            var crew = _ctx.Crews.Single();
            Assert.NotNull(crew.JobbersLastSeen);
        }

        [Fact]
        public void Should_Not_Set_Jobbers_Last_Seen_For_No_Jobbers()
        {
            _crawler.GeneratePageScrapeRequests();
            _crawler.ProcessPageScrapes();

            var crew = _ctx.Crews.Single();
            Assert.Null(crew.JobbersLastSeen);
        }

        [Fact]
        public void Should_Add_New_Pirates()
        {
            _parser.Setup(x => x.ParsePage(It.IsAny<string>())).Returns(new ParsedCrewPage()
            {
                CrewName = "Bob's Pirates",
                JobbingPirates = new List<string> { "Bob" }
            });

            _crawler.GeneratePageScrapeRequests();
            _crawler.ProcessPageScrapes();

            Assert.Single(_ctx.Pirates);
        }

        [Fact]
        public void Should_Not_Try_To_Add_Known_Pirates_Again()
        {
            _ctx.Pirates.Add(new Pirate { PirateName = "Bob" });
            _ctx.SaveChanges();

            _parser.Setup(x => x.ParsePage(It.IsAny<string>())).Returns(new ParsedCrewPage()
            {
                CrewName = "Bob's Pirates",
                JobbingPirates = new List<string> { "Bob", "Tim" }
            });

            _crawler.GeneratePageScrapeRequests();
            _crawler.ProcessPageScrapes();

            Assert.Equal(2, _ctx.Pirates.ToList().Count);
        }

        [Fact]
        public void Should_Mark_Crews_As_Deleted()
        {
            _parser.Setup(x => x.ParsePage(It.IsAny<string>())).Returns(new ParsedCrewPage()
            {
                CrewDoesNotExist = true
            });

            _crawler.GeneratePageScrapeRequests();
            _crawler.ProcessPageScrapes();

            var crew = _ctx.Crews.Single();
            Assert.NotNull(crew.DeletedDate);
        }
    }
}