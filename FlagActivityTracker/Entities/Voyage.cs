using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagActivityTracker.Entities
{
    public class Voyage
    {
        [Key]
        public int VoyageId { get; set; }
        public int CrewId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Crew Crew { get; set; }
        public List<JobbingActivity> JobbingActivities { get; set; }
    }
}
