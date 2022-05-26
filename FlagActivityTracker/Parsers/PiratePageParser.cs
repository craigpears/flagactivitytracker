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

            string[] piracySkillNames = { "Sailing", "Rigging", "Carpentry", "Patching", "Bilging", "Gunning", "Treasure Haul", "Navigating", "Battle Navigation", "Swordfighting", "Rumble" };
            string[] piracyCarousingSkills = { "Drinking", "Spades", "Hearts", "Treasure Drop", "Poker" };
            string[] piracyCraftingSkills = { "Distilling", "Alchemistry", "Shipwrightery", "Blacksmithing", "Foraging", "Weaving" };

            parsedPiratePage.Stats.AddRange(piracySkillNames.Select(x =>
                new Skill
                {
                    SkillType = SkillCategory.Piracy,
                    SkillName = x
                }
            ));

            parsedPiratePage.Stats.AddRange(piracyCarousingSkills.Select(x =>
                new Skill
                {
                    SkillType = SkillCategory.Carousing,
                    SkillName = x
                }
            ));

            parsedPiratePage.Stats.AddRange(piracyCraftingSkills.Select(x =>
                new Skill
                {
                    SkillType = SkillCategory.Crafting,
                    SkillName = x
                }
            ));

            foreach (var stat in parsedPiratePage.Stats)
            {
                var statImg = xmlDoc.SelectSingleNode($"//img[contains(@alt,'{stat.SkillName}')]");
                var fontNode = statImg.ParentNode.ParentNode.NextSibling.ChildNodes[0];
                var numberOfChildNodes = fontNode.ChildNodes.Count;
                if (numberOfChildNodes == 1)
                {
                    // Deal with stats like NoviceAble where none are in bold
                    var words =
                        Regex.Matches(fontNode.InnerText, @"([A-Z][a-z]+)")
                        .Cast<Match>()
                        .Select(m => m.Value)
                        .ToArray();

                    var experienceName = words[0];
                    var rankName = words[1];

                    stat.Experience = Enum.Parse<PiracyExperience>(experienceName);
                    stat.Rank = Enum.Parse<PiracyRank>(rankName);
                }
                else
                {
                    // Deal with stats where one or both are in bold
                    var experienceName = StripTags(fontNode.ChildNodes[0].InnerText);
                    var rankName = StripTags(fontNode.ChildNodes[numberOfChildNodes - 1].InnerText);

                    stat.Experience = Enum.Parse<PiracyExperience>(experienceName);
                    stat.Rank = Enum.Parse<PiracyRank>(rankName);
                }

            }

            return parsedPiratePage;
        }

        public string StripTags(string str)
        {
            return str.Replace("/", "").Replace("<b>", "").Replace("</b", "").Replace("\r\n", "").Replace("-","").Trim();
        }
    }
}
