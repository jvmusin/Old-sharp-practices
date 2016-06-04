using System;
using helpers;

namespace task1992_cvs
{
    public class Query
    {
        public Command Command { get; }
        public int CloneNumber { get; }
        public int SkillNumber { get; }

        public Query(Command command, int cloneNumber, int skillNumber = -1)
        {
            Command = command;
            CloneNumber = cloneNumber;
            SkillNumber = skillNumber;
        }

        public static Query Parse(string s)
        {
            var parts = s.Split();

            var command = (Command) Enum.Parse(typeof (Command), parts[0].Capitalize());
            var cloneNumber = int.Parse(parts[1]);
            var skillNumber = parts.Length == 3 ? int.Parse(parts[2]) : -1;

            return new Query(command, cloneNumber, skillNumber);
        }
    }
}
