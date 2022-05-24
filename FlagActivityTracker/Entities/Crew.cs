using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagActivityTracker.Entities
{
    public class Crew : BaseEntity
    {
        [Key]
        public int CrewId { get; set; }
        public int? FlagId { get; set; }
        public int PPCrewId { get; set; }
        public string? CrewName { get; set; }
        public DateTime? JobbersLastSeen { get; set; }
        public Flag? Flag { get; set; }
        public List<Pirate> Pirates = new();
        public List<JobbingActivity> JobbingActivities { get; set; } = new List<JobbingActivity>();
        public List<Voyage> Voyages { get; set; } = new List<Voyage>();
    }
}
