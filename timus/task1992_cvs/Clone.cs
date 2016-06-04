namespace task1992_cvs
{
    public class Clone
    {
        private Skill LastLearnedSkill { get; set; }
        private Skill LastRollback { get; set; }

        public static Clone Empty { get; } = new Clone(Skill.Empty, Skill.Empty);

        public Clone(Skill lastLearnedSkill, Skill lastRollback)
        {
            LastLearnedSkill = lastLearnedSkill;
            LastRollback = lastRollback;
        }

        public Clone(Clone parent) : this(parent.LastLearnedSkill, parent.LastRollback)
        {
        }

        public void Learn(int skillNumber)
        {
            LastLearnedSkill = LastLearnedSkill.ContinueBy(skillNumber);
            LastRollback = Skill.Empty;
        }

        public void Rollback()
        {
            LastRollback = LastRollback.ContinueBy(LastLearnedSkill);
            LastLearnedSkill = LastLearnedSkill.PreviousSkill;
        }

        public void Relearn()
        {
            LastLearnedSkill = LastLearnedSkill.ContinueBy(LastRollback);
            LastRollback = LastRollback.PreviousSkill;
        }

        public int Check()
        {
            return LastLearnedSkill.Number;
        }
    }
}
