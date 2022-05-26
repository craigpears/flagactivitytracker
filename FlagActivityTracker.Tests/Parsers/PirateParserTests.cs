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

        [Fact]
        public void Should_Parse_Skills()
        {
            var pirateParser = new PiratePageParser();
            var umamiPirateHtml = File.ReadAllText("./Parsers/Samples/umami.pirate.html");
            var parsedPiratePage = pirateParser.ParsePage(umamiPirateHtml);
            Assert.Equal(11, parsedPiratePage.Stats.Where(x => x.SkillType == SkillCategory.Piracy).Count());

            Assert.Equal(PiracyExperience.Narrow, parsedPiratePage.Stats.Single(x => x.SkillName == "Rumble").Experience);
            Assert.Equal(PiracyRank.GrandMaster, parsedPiratePage.Stats.Single(x => x.SkillName == "Rumble").Rank);

            Assert.Equal(PiracyExperience.Illustrious, parsedPiratePage.Stats.Single(x => x.SkillName == "Sailing").Experience);
            Assert.Equal(PiracyRank.Ultimate, parsedPiratePage.Stats.Single(x => x.SkillName == "Sailing").Rank);

            Assert.Equal(5, parsedPiratePage.Stats.Where(x => x.SkillType == SkillCategory.Carousing).Count());
            Assert.Equal(PiracyExperience.Novice, parsedPiratePage.Stats.Single(x => x.SkillName == "Poker").Experience);
            Assert.Equal(PiracyRank.Able, parsedPiratePage.Stats.Single(x => x.SkillName == "Poker").Rank);

            Assert.Equal(6, parsedPiratePage.Stats.Where(x => x.SkillType == SkillCategory.Crafting).Count());
            Assert.Equal(PiracyExperience.Solid, parsedPiratePage.Stats.Single(x => x.SkillName == "Alchemistry").Experience);
            Assert.Equal(PiracyRank.Master, parsedPiratePage.Stats.Single(x => x.SkillName == "Alchemistry").Rank);
        }
    }
}