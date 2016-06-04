using System.Numerics;
using NUnit.Framework;
using task1036_lucky_tickets;

namespace testers
{
    [TestFixture]
    public class Task1036Tester : AssertionHelper
    {
        public void Test(int numberLength, int sum, BigInteger result)
        {
            Expect(Solver.Solve(numberLength, sum), EqualTo(result));
        }

        [Test]
        public void Sample()
        {
            Test(2, 2, new BigInteger(4));
        }

        [Test]
        public void NotEvenSum()
        {
            Test(2, 3, BigInteger.Zero);
        }

        [Test]
        public void ZeroSum()
        {
            Test(123, 0, BigInteger.One);
        }

        [Test]
        public void BigAnswer()
        {
            Test(50, 500, BigInteger.Parse("854559745684320697549060368131279814466643179689928095831053239604130293492672614469791533133321"));
        }
    }
}
