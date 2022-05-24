using FlagActivityTracker.Entities;
using FlagActivityTracker.Models;
using FlagActivityTracker.Parsers;
using Xunit;

namespace FlagActivityTracker.Tests.Parsers
{
    public class PirateParserTests
    {
        [Fact]
        public void Should_Parse_Pirates()
        {
            var pirateParser = new PiratePageParser();
            var umamiPirateHtml = File.ReadAllText("./Parsers/Samples/umami.pirate.html");
            var parsedPiratePage = pirateParser.ParsePage(umamiPirateHtml);

            Assert.Equal("Umami", parsedPiratePage.PirateName);
            Assert.Equal(5035042, parsedPiratePage.PPCrewId);
            Assert.Equal(10006531, parsedPiratePage.PPFlagId);
        }

        [Fact]
        public void Should_Parse_Pirates_Without_Crews()
        {
            var pirateParser = new PiratePageParser();
            var crewlessPiratePageHtml = File.ReadAllText("./Parsers/Samples/no_crew.pirate.html");
            var crewlessPiratePage = pirateParser.ParsePage(crewlessPiratePageHtml);

            Assert.Null(crewlessPiratePage.PPCrewId);
            Assert.Null(crewlessPiratePage.PPFlagId);
        }

        [Fact]
        public void Should_Parse_Pirates_Without_Flags()
        {
            var pirateParser = new PiratePageParser();
            var flaglessPiratePageHtml = File.ReadAllText("./Parsers/Samples/no_flag.pirate.html");
            var flaglessPiratePage = pirateParser.ParsePage(flaglessPiratePageHtml);

            Assert.Equal(5033535, flaglessPiratePage.PPCrewId);
            Assert.Null(flaglessPiratePage.PPFlagId);
        }
    }
}