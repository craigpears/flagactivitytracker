namespace FlagActivityTracker.Models
{
    public class Skill
    {
        public SkillCategory SkillType { get; set; }
        public string SkillName { get; set; }
        public PiracyRank Rank { get; set; }
        public PiracyExperience Experience { get; set; }
    }
}
