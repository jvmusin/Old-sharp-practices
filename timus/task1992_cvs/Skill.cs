namespace task1992_cvs
{
    public class Skill
    {
        public Skill PreviousSkill { get; }
        public int Number { get; }

        public static Skill Empty { get; } = new Skill(null, -1);

        private Skill(Skill previousSkill, int number)
        {
            PreviousSkill = previousSkill;
            Number = number;
        }

        public Skill ContinueBy(Skill skill)
        {
            return ContinueBy(skill.Number);
        }

        public Skill ContinueBy(int skillNumber)
        {
            return new Skill(this, skillNumber);
        }
    }
}
