using FlagActivityTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace FlagActivityTracker.Parsers
{
    public interface IFlagPageParser
    {
        ParsedFlagPage ParsePage(string flagPageHtml);
    }

    public class FlagPageParser : IFlagPageParser
    {
        public ParsedFlagPage ParsePage(string flagPageHtml)
        {
            var parsedFlagPage = new ParsedFlagPage();

            var xmlDoc = PageParserHelper.ParsePage(flagPageHtml);
            var flagNameCell = xmlDoc.SelectSingleNode("//body/table[1]/tr[1]/td[1]/table[1]/tr[2]/td[3]/font[1]/b");
            parsedFlagPage.FlagName = flagNameCell.InnerText;

            var crewAnchorTagNodes = xmlDoc.SelectNodes("//a[contains(@href,'/yoweb/crew/')]");

            foreach (XmlNode crewAnchorTagNode in crewAnchorTagNodes)
            {
                var crewLinkHref = crewAnchorTagNode.Attributes["href"].Value;
                var ppCrewId = int.Parse(new Regex("[0-9]+").Match(crewLinkHref).Value);
                parsedFlagPage.PuzzlePirateCrewIds.Add(ppCrewId);
            }

            return parsedFlagPage;
        }
    }
}
