using System.Collections.Generic;
using NUnit.Framework;
using task1067_disk_tree;

namespace testers
{
    [TestFixture]
    public class Task1067Tester : AssertionHelper
    {
        public void Test(IEnumerable<string> directories, IEnumerable<string> output)
        {
            Expect(Solver.Solve(directories), EqualTo(output));
        }

        [Test]
        public void Sample()
        {
            var input = new[]
            {
                @"WINNT\SYSTEM32\CONFIG",
                @"GAMES",
                @"WINNT\DRIVERS",
                @"HOME",
                @"WIN\SOFT",
                @"GAMES\DRIVERS",
                @"WINNT\SYSTEM32\CERTSRV\CERTCO~1\X86"
            };
            var output = new[]
            {
                @"GAMES",
                @" DRIVERS",
                @"HOME",
                @"WIN",
                @" SOFT",
                @"WINNT",
                @" DRIVERS",
                @" SYSTEM32",
                @"  CERTSRV",
                @"   CERTCO~1",
                @"    X86",
                @"  CONFIG",
            };

            Test(input, output);
        }

        [Test]
        public void FromComments()
        {
            var input = new[]
            {
                @"a\b",
                @"d\a",
                @"cc\e",
                @"e\d"
            };
            var output = new[]
            {
                @"a",
                @" b",
                @"cc",
                @" e",
                @"d",
                @" a",
                @"e",
                @" d"
            };
            Test(input, output);
        }

        [Test]
        public void OneDirectory()
        {
            Test(new[] {"dir"}, new[] {"dir"});
        }
    }
}
