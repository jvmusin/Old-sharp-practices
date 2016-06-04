using System.Collections.Generic;
using NUnit.Framework;
using task1494_monobilliards;

namespace testers
{
    [TestFixture]
    public class Task1494Tester : AssertionHelper
    {
        private void Test(IReadOnlyList<int> takingOrder, bool isCheater)
        {
            Expect(Solver.IsCheater(takingOrder), EqualTo(isCheater));
        }

        [Test]
        public void Sample1()
        {
            Test(new[] {2, 1}, false);
        }

        [Test]
        public void Sample2()
        {
            Test(new[] {3, 1, 2}, true);
        }

        [Test]
        public void SystemTest11()
        {
            Test(new[] {1, 3, 4, 2}, false);
        }

        [Test]
        public void FromComments1()
        {
            Test(new[] {2, 4, 3, 1, 5}, false);
        }

        [Test]
        public void FromComments2()
        {
            Test(new[] {2, 4, 1, 3, 5}, true);
        }
    }
}
