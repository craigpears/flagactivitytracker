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
    public interface IFlagPageParser : IDisposable
    {
        ParsedFlagPage? DownloadFlagPage(int puzzlePiratesFlagId);
        string DownloadFlagPageHtml(int puzzlePiratesFlagId);
        ParsedFlagPage ParsePage(string flagPageHtml);
    }

    public class FlagPageParser : IFlagPageParser
    {
        protected WebClient _webClient;

        public FlagPageParser()
        {
            _webClient = new WebClient();
        }

        public void Dispose()
        {
            if (_webClient != null)
                _webClient.Dispose();
        }
        public ParsedFlagPage? DownloadFlagPage(int puzzlePiratesFlagId)
        {
            // TODO: Audit the downloaded HTML pages in the database?
            var pageHtml = DownloadFlagPageHtml(puzzlePiratesFlagId);
            if (pageHtml == "")
                return null;

            var parsedFlagPage = ParsePage(pageHtml);
            return parsedFlagPage;
        }

        public string DownloadFlagPageHtml(int puzzlePiratesFlagId)
        {
            var pageUrl = $"https://emerald.puzzlepirates.com/yoweb/flag/info.wm?flagid={puzzlePiratesFlagId}&classic=false";
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
