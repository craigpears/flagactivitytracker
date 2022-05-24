// See https://aka.ms/new-console-template for more information
using FlagActivityTracker;
using FlagActivityTracker.Crawlers;
using FlagActivityTracker.Database;
using FlagActivityTracker.Entities;
using FlagActivityTracker.Parsers;
using FlagActivityTracker.Services;
using System.Net;

var context = new FlagActivityTrackerDbContext();

Console.WriteLine("Hello, Pirates!");
// Ideas to track:
/*
 *  Rate at which experience is gained
 *  How many puzzles they typically do
 *  Which puzzles they typically do
 *  How long they're typically on for
 *  Amount of time online spent jobbing/not jobbing
 *  Average ranks
 *  What is the average size voyage they participate in?
 *  When do they join the voyage?
 *  Graph of who they go on voyages with
 *  Time pirate first seen - did they all get first seen around similar times? (bulk creating new pirates after banning)
 *  Track mass pirate disappearance events (large number of previously active accounts all dropping off at a similar point in time) + watch for new accounts created shortly after?
 *  The size of their main crew?
 */

while(true)
{
    var pageScrapeService = new PageScrapeService(context);

    var piratePageParser = new PiratePageParser();
    var pirateCrawler = new PirateCrawler(context, piratePageParser, pageScrapeService);

    var crewPageParser = new CrewPageParser();
    var crewCrawler = new CrewCrawler(context, crewPageParser, pageScrapeService);

    var flagPageParser = new FlagPageParser();
    var flagCrawler = new FlagCrawler(context, flagPageParser, pageScrapeService);

    pirateCrawler.GeneratePageScrapeRequests();
    crewCrawler.GeneratePageScrapeRequests();
    flagCrawler.GeneratePageScrapeRequests();

    pageScrapeService.DownloadPages();

    pirateCrawler.ProcessPageScrapes();
    crewCrawler.ProcessPageScrapes();
    flagCrawler.ProcessPageScrapes();

    var voyageProcessor = new VoyageProcessor(context);
    voyageProcessor.ProcessJobbingActivity();

    // TODO: Feature Ideas
    /*
     *  War alert - send alerts for new voyages if they are flagged as at war
     *  Blockade Crews - Flag specific crews as being of interest to prioritise them, or do this based on historical max jobber counts?
     */

    Console.WriteLine("Sleeping...");
    var oneMinute = 1000 * 60;
    Thread.Sleep(oneMinute);
}

/*

-- Number of distinct pirates on a voyage
SELECT VoyageId, COUNT(PirateId) NumberOfPirates
FROM JobbingActivities
GROUP BY VoyageId

-- Crew/Flag breakdown of the jobbers on a voyage
SELECT c.CrewId, c.CrewName, f.FlagId, f.FlagName, COUNT(DISTINCT p.PirateId) Pirates
FROM JobbingActivities a
JOIN Pirates p ON a.PirateId = p.PirateId
JOIN Crews c ON p.CrewId = c.CrewId
JOIN Flags f ON f.FlagId = c.FlagId
WHERE VoyageId = 6
GROUP BY c.CrewId, c.CrewName, f.FlagId, f.FlagName
ORDER BY COUNT(DISTINCT p.PirateId) DESC 

-- Crews with most pirates on voyages
;WITH VoyagesData AS (
	SELECT v.VoyageId, v.CrewId, COUNT(DISTINCT ja.PirateId) PiratesCount
	FROM Voyages v
	JOIN JobbingActivities ja ON ja.VoyageId = v.VoyageId
	GROUP BY v.VoyageId, v.CrewId
)
SELECT c.CrewId, c.CrewName, COUNT(DISTINCT v.VoyageID) VoyagesCount, SUM(PiratesCount) TotalPirates
FROM VoyagesData v
JOIN Crews c ON c.CrewId = v.CrewId
GROUP BY c.CrewId, c.CrewName
ORDER BY SUM(PiratesCount) DESC

-- Active Crews
SELECT c.CrewId, c.CrewName, COUNT(*) ActivePiratesCount
FROM Crews c
JOIN Pirates p ON c.CrewId = p.CrewId
GROUP BY c.CrewId, c.CrewName
ORDER BY COUNT(*) DESC

*/

