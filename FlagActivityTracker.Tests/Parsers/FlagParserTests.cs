using FlagActivityTracker.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FlagActivityTracker.Tests.Parsers
{
    public class FlagParserTests
    {
        [Fact]
        public void Should_Parse_Flag_Name()
        {
            var flagParser = new FlagPageParser();
            var flagPageHtml = File.ReadAllText("./Parsers/Samples/poorlifedecisions.flag.html");
            var flag = flagParser.ParsePage(flagPageHtml);
            Assert.Equal("Poor Life Decisions", flag.FlagName);
        }

        [Fact]
        public void Should_Parse_Crew_List()
        {
            var flagParser = new FlagPageParser();
            var flagPageHtml = File.ReadAllText("./Parsers/Samples/poorlifedecisions.flag.html");
            var flag = flagParser.ParsePage(flagPageHtml);

            Assert.Equal(12, flag.PuzzlePirateCrewIds.Count);
        }
    }
}
