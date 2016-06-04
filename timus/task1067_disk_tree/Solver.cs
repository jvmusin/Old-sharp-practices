using System.Collections.Generic;
using System.Linq;
using static helpers.Helpers;

namespace task1067_disk_tree
{
    public class Solver
    {
        public static void Main()
        {
            var queryCount = ReadInt();
            var directories = Enumerable.Range(0, queryCount).Select(i => ReadLine());
            var solution = Solve(directories);

            foreach (var result in solution)
                WriteLine(result);
        }

        public static IEnumerable<string> Solve(IEnumerable<string> directories)
        {
            var root = new Directory("root");
            directories
                .Select(command => command.Split('\\'))
                .Select(path => path.Aggregate(root, (current, next) => current.ContinueBy(next)))
                .ToArray();

            return root.EnumerateTree();
        }
    }
}
