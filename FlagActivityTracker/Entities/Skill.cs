using System.ComponentModel.DataAnnotations;

namespace FlagActivityTracker.Entities
{
    public class Skill
    {
        [Key]
        public int SkillId { get; set; }
        public int PirateId { get; set; }
        public SkillCategory SkillType { get; set; }
        public string SkillName { get; set; }
        public SkillRating Rating { get; set; }
        public SkillExperience Experience { get; set; }
        public Pirate Pirate { get; set; } = new();
    }
}
