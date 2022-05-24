using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagActivityTracker.Models
{
    public class ParsedCrewPage
    {
        public string CrewName { get; set; }
        public List<String> JobbingPirates { get; set; } = new List<String>();
    }
}
