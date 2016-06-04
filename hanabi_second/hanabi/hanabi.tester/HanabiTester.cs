using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace hanabi.tester
{
    [TestFixture]
    public class HanabiTester : AssertionHelper
    {
        [Test]
        [TestCaseSource(nameof(Tests))]
        public IEnumerable<string> Test(IEnumerable<string> commands)
        {
            var hanabi = new HanabiInteractive();
            return commands
                .Select(hanabi.MakeTurn)
                .Where(result => result != null)
                .Select(result => result.ToString());
        }

        private static IEnumerable<TestCaseData> Tests
        {
            get
            {
                yield return PrepareTestCaseData("1-1");
                yield return PrepareTestCaseData("1-2");
                yield return PrepareTestCaseData("1-big");
                yield return PrepareTestCaseData("2-1");
                yield return PrepareTestCaseData("2-big");
                yield return PrepareTestCaseData("table_filled");
            }
        }

        private static TestCaseData PrepareTestCaseData(string testName)
        {
            return new TestCaseData(
                ReadFile(testName + ".in"))
                .Returns(ReadFile(testName + ".out"));
        }

        private static IEnumerable<string> ReadFile(string fileName)
        {
            const string directory = @"C:\tests\";
            return File.ReadAllLines(directory + fileName);
        }
    }
}
