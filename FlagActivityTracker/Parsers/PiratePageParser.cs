using FlagActivityTracker.Entities;
using FlagActivityTracker.Models;
using Sgml;
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
    public interface IPiratePageParser
    {
        ParsedPiratePage ParsePage(string piratePageHtml);
    }

    public class PiratePageParser : IPiratePageParser
    {
        public ParsedPiratePage ParsePage(string piratePageHtml)
        {
            var parsedPiratePage = new ParsedPiratePage();

            var xmlDoc = PageParserHelper.ParsePage(piratePageHtml);
            var pirateNameCell = xmlDoc.SelectNodes("//body/table/tr/td/table/tr/td/font/b")[1];
            parsedPiratePage.PirateName = pirateNameCell.InnerText;

            if(xmlDoc.InnerText.Contains("Independent Pirate"))
            {
                parsedPiratePage.IndependentPirate = true;
                return parsedPiratePage;
            }

            string pirateSummaryTable = "//body/table/tr/td[1]/table/tr[2]/td[1]/table";
            var crewLinkNode = xmlDoc.SelectSingleNode($"{pirateSummaryTable}/tr[1]/td/font/b/a");
            var flagLinkNode = xmlDoc.SelectSingleNode($"{pirateSummaryTable}/tr[2]/td/font/b/a");

            if(crewLinkNode != null)
            {
                var crewLinkHref = crewLinkNode.Attributes["href"].Value;
                parsedPiratePage.PPCrewId = int.Parse(new Regex("[0-9]+").Match(crewLinkHref).Value);
            }

            if(flagLinkNode != null)
            {
                var flagLinkHref = flagLinkNode.Attributes["href"].Value;
                parsedPiratePage.PPFlagId = int.Parse(new Regex("[0-9]+").Match(flagLinkHref).Value);
            }

            return parsedPiratePage;
        }
    }
}
