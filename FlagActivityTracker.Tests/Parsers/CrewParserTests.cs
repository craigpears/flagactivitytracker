using FlagActivityTracker.Entities;
using FlagActivityTracker.Parsers;
using Xunit;

namespace FlagActivityTracker.Tests.Parsers
{
    public class CrewParserTests
    {
        public CrewParserTests()
        {

        }

        [Fact]
        public void Should_Parse_Crew_Name()
        {
            var crewParser = new CrewPageParser();
            var crewPageHtml = File.ReadAllText("./Parsers/Samples/reverie.crew.html");
            var crew = crewParser.ParsePage(crewPageHtml);
            Assert.Equal("Reverie", crew.CrewName);
        }

        [Fact]
        public void Should_Show_No_Jobbers_When_There_Are_None()
        {
            var crewParser = new CrewPageParser();
            var crewPageHtml = File.ReadAllText("./Parsers/Samples/reverie.crew.html");
            var crew = crewParser.ParsePage(crewPageHtml);
            Assert.Equal(0, crew.JobbingPirates.Count);
        }

        [Fact]
        public void Should_Find_Single_Jobbers()
        {
            var crewParser = new CrewPageParser();
            var crewPageHtml = File.ReadAllText("./Parsers/Samples/reverie_with_jobber.crew.html");
            var crewWithJobber = crewParser.ParsePage(crewPageHtml);

            Assert.Single(crewWithJobber.JobbingPirates);
            Assert.Equal("Chimpsalt", crewWithJobber.JobbingPirates[0]);
        }

        [Fact]
        public void Should_Find_Multiple_Jobbers()
        {
            var crewParser = new CrewPageParser();
            var crewPageHtml = File.ReadAllText("./Parsers/Samples/reverie_with_multiple_jobbers.crew.html");
            var crewWithJobber = crewParser.ParsePage(crewPageHtml);

            Assert.Equal(2, crewWithJobber.JobbingPirates.Count);
            Assert.Equal("Chimpsalt", crewWithJobber.JobbingPirates[0]);
            Assert.Equal("Chimpysalt", crewWithJobber.JobbingPirates[1]);
        }

        [Fact]
        public void Should_Find_Jobbers_When_There_Are_Multiple_Rows()
        {
            var crewParser = new CrewPageParser();
            var crewPageHtml = File.ReadAllText("./Parsers/Samples/angels_with_multiple_jobber_rows.crew.html");
            var crewWithManyJobbers = crewParser.ParsePage(crewPageHtml);

            Assert.Equal(10, crewWithManyJobbers.JobbingPirates.Count);
        }

        [Fact]
        public void Should_Identify_Crew_Does_Not_Exist_Messages()
        {
            var crewParser = new CrewPageParser();
            var crewPageHtml = File.ReadAllText("./Parsers/Samples/no_such_crew.crew.html");
            var noSuchCrew = crewParser.ParsePage(crewPageHtml);

            Assert.True(noSuchCrew.CrewDoesNotExist);
        }
    }
}