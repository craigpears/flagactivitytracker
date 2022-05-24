using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagActivityTracker.Models
{
    public class ParsedFlagPage
    {
        public string FlagName { get; set; }
        public List<int> PuzzlePirateCrewIds { get; set; } = new List<int>();
    }
}
