using FlagActivityTracker.Crawlers;
using FlagActivityTracker.Database;
using FlagActivityTracker.Entities;
using FlagActivityTracker.Models;
using FlagActivityTracker.Parsers;
using Moq;
using Xunit;

namespace FlagActivityTracker.Tests.Crawlers
{
    public class PirateCrawlerTests
    {
        protected FlagActivityTrackerDbContext _ctx;
        protected PirateCrawler _crawler;
        protected Mock<IPiratePageParser> _parser;

        public PirateCrawlerTests()
        {            
            _ctx = new FlagActivityTrackerDbContext(useInMemoryDatabase: true);
            _parser = new Mock<IPiratePageParser>();
            _crawler = new PirateCrawler(_ctx, _parser.Object);

            _ctx.Pirates.Add(new Pirate { PirateName = "Bob" });
            _parser.Setup(x => x.DownloadPiratePage(It.Is<string>(x => x == "Bob"))).Returns(new ParsedPiratePage
            {
                PirateName = "Bob",
                PPCrewId = 123,
                PPFlagId = 456
            });

            _ctx.SaveChanges();
        }

        [Fact]
        public void Should_Create_And_Link_Missing_Crews()
        {
            _crawler.PopulatePirates();

            Assert.Single(_ctx.Pirates);
            Assert.Single(_ctx.Flags);
            Assert.Single(_ctx.Crews);

            var pirate = _ctx.Pirates.Single();
            Assert.Equal("Bob", pirate.PirateName);
            Assert.Equal(123, pirate?.Crew?.PPCrewId);
            Assert.Equal(456, pirate?.Crew?.Flag?.PPFlagId);
            Assert.NotNull(pirate?.LastParsedDate);
            Assert.Null(pirate?.LastErrorDate);
            Assert.Equal(0, pirate?.ErrorCount);
        }

        [Fact]
        public void Should_Only_Populate_Unprocessed_Pirates()
        {
            _ctx.Pirates.Single().LastParsedDate = DateTime.MinValue;
            _ctx.SaveChanges();

            _crawler.PopulatePirates();

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
            _parser.Setup(x => x.DownloadPiratePage(It.Is<string>(x => x == "Bob"))).Returns(new ParsedPiratePage
            {
                PirateName = "Bob",
                PPCrewId = 123,
                PPFlagId = null
            });

            _crawler.PopulatePirates();

            Assert.Single(_ctx.Pirates);
            Assert.Empty(_ctx.Flags);
            Assert.Single(_ctx.Crews);

            var pirate = _ctx.Pirates.Single();
            Assert.Equal("Bob", pirate.PirateName);
            Assert.Equal(123, pirate?.Crew?.PPCrewId);
            Assert.Null(pirate?.Crew?.Flag);
            Assert.NotNull(pirate?.LastParsedDate);
        }

        [Fact]
        public void Should_Skip_Pirates_With_Missing_Pirate_Page_Info()
        {
            _parser.Setup(x => x.DownloadPiratePage(It.Is<string>(x => x == "Bob"))).Returns<ParsedPiratePage?>(null);

            _crawler.PopulatePirates();

            var pirate = _ctx.Pirates.Single();
            Assert.Single(_ctx.Pirates);
            Assert.Empty(_ctx.Flags);
            Assert.Empty(_ctx.Crews);
            Assert.Null(pirate?.LastParsedDate);
            Assert.NotNull(pirate?.LastErrorDate);
            Assert.Equal(1, pirate?.ErrorCount);
        }

        [Fact]
        public void Should_Process_Pirates_Five_Minutes_After_Errors()
        {
            _ctx.Pirates.Single().LastErrorDate = DateTime.UtcNow;
            _ctx.SaveChanges();

            // Should do nothing first time
            _crawler.PopulatePirates();
            Assert.Null(_ctx.Pirates.Single()?.LastParsedDate);

            // Should update when it's past the five minute mark and clear the error count
            _ctx.Pirates.Single().LastErrorDate = DateTime.UtcNow.AddMinutes(-6);
            _ctx.SaveChanges();

            _crawler.PopulatePirates();
            var pirate = _ctx.Pirates.Single();
            Assert.NotNull(pirate?.LastParsedDate);
            Assert.Equal(0, pirate?.ErrorCount);
        }
    }
}