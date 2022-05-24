using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagActivityTracker.Entities
{
    public class PageScrape
    {
        [Key]
        public int PageScrapeId { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string PuzzlePiratesId { get; set; }
        public string? PageUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public PageType PageType { get; set; }
        public bool Processed { get; set; }
        public string? DownloadedHtml { get; set; }
        public DateTime? DownloadedDate { get; set; }
        public int Attempts { get; set; }
    }
}
