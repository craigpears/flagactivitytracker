using FlagActivityTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagActivityTracker.Models
{
    public class ParsedPiratePage
    {
        public string PirateName { get; set; }
        public int? PPCrewId { get; set; }
        public int? PPFlagId { get; set; }
        public bool IndependentPirate { get; set; }
        public List<Skill> Skills { get; set; } = new();
    }
}
