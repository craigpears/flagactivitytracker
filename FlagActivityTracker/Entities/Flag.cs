using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagActivityTracker.Entities
{
    public class Flag : BaseEntity
    {
        [Key]
        public int FlagId { get; set; }
        public int PPFlagId { get; set; }
        public string? FlagName { get; set; }
        public List<Crew> Crews = new();
    }
}
