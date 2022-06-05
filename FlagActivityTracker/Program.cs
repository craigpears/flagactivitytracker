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
 *  Do they do anything other than piracy?
 *  What rank/crew type are they in - are they a decent rank in a large crew/flag or in a crew/flag all by themselves?
 *  Do the pirates have portraits?
 *  Do they job in blockades?  
 *  How long do they job for in blockades, or generally stay on a voyage?
 *  Do they ever leave a voyage part way through or do they always stay?
 *  Do they start jobbing during spikes that happen on one side of a blockade but not the other?
 *  What houses are they roommates in or own - are there any expensive ones?
 *  What trophys do they have?  Older pirates with 10 year trophies are unlikely to be bots
 *  Do they have familiars?  If they do then it's unlikely to be a bot
 *  Do they own any stalls/shops?
 *  Do they have high experience/stats with other novice ables?
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
     *  
     *  Dashboard with Signalr - show in real time jobber count jumps, activity feed with jobbers appearing + what flag they're from
     *  
     *  Add distinct pirate counts to voyages + max distinct jobbers at any one time
     *  Scan flag pages to choose which crews to scan to save requests + scan more crews
     */

    Console.WriteLine("Sleeping...");
    var oneMinute = 1000 * 60;
    Thread.Sleep(oneMinute);
}


/*
 * 
 
-- Minute by minute jobbing levels for voyages
;WITH ActivityDates AS (
	SELECT DISTINCT DATEADD(minute, DATEDIFF(minute, 0, ActivityDate), 0) RoundedActivityDate 
	FROM JobbingActivities ja 
	WHERE VoyageId IN (596, 588)
)
SELECT RoundedActivityDate,
(SELECT COUNT(DISTINCT PirateId) FROM JobbingActivities WHERE RoundedActivityDate = DATEADD(minute, DATEDIFF(minute, 0, ActivityDate), 0) AND VoyageId = 596) 'Souless Hunters Jobbers',
(SELECT COUNT(DISTINCT PirateId) FROM JobbingActivities WHERE RoundedActivityDate = DATEADD(minute, DATEDIFF(minute, 0, ActivityDate), 0) AND VoyageId = 588) 'Norse Warriors Jobbers'
FROM ActivityDates ad
ORDER BY RoundedActivityDate DESC

-- Voyage breakdown by flag
SELECT f.FlagName, COUNT(DISTINCT p.PirateId) Jobbers
FROM JobbingActivities ja
JOIN Pirates p ON ja.PirateId = p.PirateID
JOIN Crews c ON c.CrewId = p.CrewId
JOIN Flags f ON f.FlagId = c.FlagId
WHERE VoyageID = 596
GROUP BY f.FlagName
ORDER BY COUNT(DISTINCT p.PirateId) DESC

-- Voyage jobber quality breakdown by flag
SELECT f.FlagName, COUNT(DISTINCT p.PirateId) Jobbers, SUM(s.Rating) TotalJobberQuality
FROM JobbingActivities ja
JOIN Pirates p ON ja.PirateId = p.PirateID
JOIN Skill s ON s.PirateId = p.PirateId
JOIN Crews c ON c.CrewId = p.CrewId
JOIN Flags f ON f.FlagId = c.FlagId
WHERE VoyageID = 588
AND s.SkillType = 0
GROUP BY f.FlagName
ORDER BY SUM(s.Rating) DESC

-- TODO: Breakdowns by round?

*/