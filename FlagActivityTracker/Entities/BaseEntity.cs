using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagActivityTracker.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastParsedDate { get; set; }
        public DateTime? LastErrorDate { get; set; }
        public int ErrorCount { get; set; }
    }
}
