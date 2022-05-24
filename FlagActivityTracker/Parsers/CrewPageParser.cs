using FlagActivityTracker.Entities;
using FlagActivityTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FlagActivityTracker.Parsers
{
    public interface ICrewPageParser
    {
        ParsedCrewPage ParsePage(string crewPageHtml);
    }

    public class CrewPageParser : ICrewPageParser
    {
        public ParsedCrewPage ParsePage(string crewPageHtml)
        {
            var parsedCrewPage = new ParsedCrewPage();

            var xmlDoc = PageParserHelper.ParsePage(crewPageHtml);
            var crewNameCell = xmlDoc.SelectSingleNode("//body/table[1]/tr[1]/td[1]/table[1]/tr[2]/td[3]/font[1]/b");
            parsedCrewPage.CrewName = crewNameCell.InnerText;

            var jobbingPirateTitleCellRow = xmlDoc.SelectSingleNode("//tr[td/table/tr/td/b/text()[contains(., 'Jobbing Pirate')]]");
            if (jobbingPirateTitleCellRow != null)
            {
                var nextSibling = jobbingPirateTitleCellRow.NextSibling;
                while (nextSibling != null)
                {
                    var jobbingPirateRows = nextSibling.ChildNodes;
                    foreach (XmlNode jobbingPirateRow in jobbingPirateRows)
                    {
                        var name = jobbingPirateRow.InnerText;
                        parsedCrewPage.JobbingPirates.Add(name);
                    }

                    nextSibling = nextSibling.NextSibling;
                }
            }


            return parsedCrewPage;
        }
    }
}
