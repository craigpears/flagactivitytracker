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
            Assert.Equal(11, parsedPiratePage.Skills.Where(x => x.SkillType == SkillCategory.Piracy).Count());

            Assert.Equal(SkillExperience.Narrow, parsedPiratePage.Skills.Single(x => x.SkillName == "Rumble").Experience);
            Assert.Equal(SkillRating.GrandMaster, parsedPiratePage.Skills.Single(x => x.SkillName == "Rumble").Rating);

            Assert.Equal(SkillExperience.Illustrious, parsedPiratePage.Skills.Single(x => x.SkillName == "Sailing").Experience);
            Assert.Equal(SkillRating.Ultimate, parsedPiratePage.Skills.Single(x => x.SkillName == "Sailing").Rating);

            Assert.Equal(5, parsedPiratePage.Skills.Where(x => x.SkillType == SkillCategory.Carousing).Count());
            Assert.Equal(SkillExperience.Novice, parsedPiratePage.Skills.Single(x => x.SkillName == "Poker").Experience);
            Assert.Equal(SkillRating.Able, parsedPiratePage.Skills.Single(x => x.SkillName == "Poker").Rating);

            Assert.Equal(6, parsedPiratePage.Skills.Where(x => x.SkillType == SkillCategory.Crafting).Count());
            Assert.Equal(SkillExperience.Solid, parsedPiratePage.Skills.Single(x => x.SkillName == "Alchemistry").Experience);
            Assert.Equal(SkillRating.Master, parsedPiratePage.Skills.Single(x => x.SkillName == "Alchemistry").Rating);
        }

        [Fact]
        public void Should_Parse_Stats_When_They_Have_Trophies_For_That_Stat()
        {
            var pirateParser = new PiratePageParser();
            var rytasticPirateHtml = File.ReadAllText("./Parsers/Samples/rytastic.pirate.html");
            var parsedPiratePage = pirateParser.ParsePage(rytasticPirateHtml);

            Assert.Equal(SkillExperience.Solid, parsedPiratePage.Skills.Single(x => x.SkillName == "Treasure Haul").Experience);
            Assert.Equal(SkillRating.GrandMaster, parsedPiratePage.Skills.Single(x => x.SkillName == "Treasure Haul").Rating);
        }
    }
}