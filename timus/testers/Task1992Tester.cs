using System.Collections.Generic;
using NUnit.Framework;
using task1992_cvs;

namespace testers
{
    [TestFixture]
    public class Task1992Tester : AssertionHelper
    {
        private void Test(IEnumerable<Query> queries, IEnumerable<int> expectedResponses)
        {
            Expect(Solver.Solve(queries), EqualTo(expectedResponses));
        }

        [Test]
        public void Sample()
        {
            var queries = new[]
            {
                new Query(Command.Learn, 1, 5),
                new Query(Command.Learn, 1, 7),
                new Query(Command.Rollback, 1),
                new Query(Command.Check, 1),
                new Query(Command.Clone, 1),
                new Query(Command.Relearn, 2),
                new Query(Command.Check, 2),
                new Query(Command.Rollback, 1),
                new Query(Command.Check, 1)
            };

            Test(queries, new[] {5, 7, -1});
        }

        [Test]
        public void SystemTest11()
        {
            var queries = new[]
            {
                new Query(Command.Learn, 1, 1),
                new Query(Command.Learn, 1, 2),
                new Query(Command.Learn, 1, 3),
                new Query(Command.Learn, 1, 4),
                new Query(Command.Rollback, 1),
                new Query(Command.Rollback, 1),
                new Query(Command.Relearn, 1),
                new Query(Command.Clone, 1),
                new Query(Command.Check, 2),
                new Query(Command.Relearn, 2),
                new Query(Command.Check, 2),
                new Query(Command.Rollback, 2),
                new Query(Command.Rollback, 2),
                new Query(Command.Check, 2),
                new Query(Command.Rollback, 2),
                new Query(Command.Check, 2),
                new Query(Command.Check, 1),
                new Query(Command.Relearn, 1),
                new Query(Command.Check, 1)
            };

            Test(queries, new[] {3, 4, 2, 1, 3, 4});
        }
    }
}
