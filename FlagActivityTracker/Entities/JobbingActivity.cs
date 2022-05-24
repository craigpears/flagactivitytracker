using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagActivityTracker.Entities
{
    public class JobbingActivity
    {
        [Key]
        public int JobbingActivityId { get; set; }
        public DateTime ActivityDate { get; set; }
        public int PirateId { get; set; }
        public int CrewId { get; set; }
        public int? VoyageId { get; set; }
        public Pirate Pirate { get; set; }
        public Crew Crew { get; set; }
        public Voyage? Voyage { get; set; }
    }
}
