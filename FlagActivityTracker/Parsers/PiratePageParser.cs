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
    public interface IPiratePageParser : IDisposable
    {
        ParsedPiratePage? DownloadPiratePage(string pirateName);
        string DownloadPiratePageHtml(string pirateName);
        ParsedPiratePage ParsePage(string piratePageHtml);
    }

    public class PiratePageParser : IPiratePageParser
    {
        protected WebClient _webClient;

        public PiratePageParser()
        {
            _webClient = new WebClient();
        }

        public void Dispose()
        {
            if (_webClient != null)
                _webClient.Dispose();
        }
        public ParsedPiratePage? DownloadPiratePage(string pirateName)
        {
            // TODO: Audit the downloaded HTML pages in the database?
            var pageHtml = DownloadPiratePageHtml(pirateName);
            if (pageHtml == "")
                return null;

            var parsedPiratePage = ParsePage(pageHtml);
            return parsedPiratePage;
        }

        public string DownloadPiratePageHtml(string pirateName)
        {
            var pageUrl = $"https://emerald.puzzlepirates.com/yoweb/pirate.wm?classic=false&target={pirateName}";
            var proxyUrl = $"http://api.scraperapi.com?api_key=03339515441ba3541a70111efe98de57&url={pageUrl}";
            string pageHtml = "";
            try
            {
                pageHtml = _webClient.DownloadString(proxyUrl);
            }
            catch (Exception)
            {
                Console.WriteLine($"Exception loading {pageUrl}");
            }

            return pageHtml;
        }

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
