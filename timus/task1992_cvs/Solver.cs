using System.Collections.Generic;
using System.Linq;
using static helpers.Helpers;

namespace task1992_cvs
{
    public class Solver
    {
        public static IEnumerable<int> Solve(IEnumerable<Query> queries)
        {
            var clones = new List<Clone> { Clone.Empty };

            foreach (var query in queries)
            {
                var command = query.Command;
                var skillNumber = query.SkillNumber;
                var currentClone = clones[query.CloneNumber - 1];

                switch (command)
                {
                    case Command.Learn:
                        currentClone.Learn(skillNumber);
                        break;

                    case Command.Rollback:
                        currentClone.Rollback();
                        break;

                    case Command.Relearn:
                        currentClone.Relearn();
                        break;

                    case Command.Clone:
                        clones.Add(new Clone(currentClone));
                        break;

                    case Command.Check:
                        yield return currentClone.Check();
                        break;
                }
            }
        }

        public static void Main(string[] args)
        {
            var queryCount = ReadInt();
            var queries = Enumerable.Range(0, queryCount)
                .Select(_ => ReadLine())
                .Select(Query.Parse);

            var solution = Solve(queries)
                .Select(result => result == -1 ? "basic" : result.ToString());

            foreach (var result in solution)
                WriteLine(result);
        }
    }
}
