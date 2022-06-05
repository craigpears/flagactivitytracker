using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FlagActivityTracker.Entities
{
    public class Pirate : BaseEntity
    {
        [Key]
        public int PirateId { get; set; }
        public int? CrewId { get; set; }
        public string PirateName { get; set; }
        public Crew? Crew { get; set; }
        public List<JobbingActivity> JobbingActivities { get; set; } = new List<JobbingActivity>();
        public List<Skill> Skills { get; set; }
    }
}
