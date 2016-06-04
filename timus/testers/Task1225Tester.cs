using System.Collections;
using NUnit.Framework;
using task1225_flags;

namespace testers
{
    [TestFixture]
    public class Task1225Tester : AssertionHelper
    {
        [Test]
        [TestCaseSource(nameof(GenerateTestCases))]
        public long CheckAllTestCases(int stripeCount)
        {
            return Solver.Solve(stripeCount);
        }

        public void Test(int stripeCount, long result)
        {
            Expect(Solver.Solve(stripeCount), EqualTo(result));
        }

        [Test]
        public void Sample()
        {
            Test(3, 4);
        }

        [Test]
        public void FiveStripes()
        {
            Test(5, 10);
        }

        [Test]
        public void IntOverflow()
        {
            Test(45, 2269806340L);
        }

        public static IEnumerable GenerateTestCases()
        {
            yield return new TestCaseData(3).Returns(4);
            yield return new TestCaseData(4).Returns(6);
            yield return new TestCaseData(5).Returns(10);
            yield return new TestCaseData(6).Returns(16);
            yield return new TestCaseData(7).Returns(26);
            yield return new TestCaseData(8).Returns(42);
            yield return new TestCaseData(9).Returns(68);
            yield return new TestCaseData(10).Returns(110);
            yield return new TestCaseData(11).Returns(178);
            yield return new TestCaseData(12).Returns(288);
            yield return new TestCaseData(13).Returns(466);
            yield return new TestCaseData(14).Returns(754);
            yield return new TestCaseData(15).Returns(1220);
            yield return new TestCaseData(16).Returns(1974);
            yield return new TestCaseData(17).Returns(3194);
            yield return new TestCaseData(18).Returns(5168);
            yield return new TestCaseData(19).Returns(8362);
            yield return new TestCaseData(20).Returns(13530);
            yield return new TestCaseData(21).Returns(21892);
            yield return new TestCaseData(22).Returns(35422);
            yield return new TestCaseData(23).Returns(57314);
            yield return new TestCaseData(24).Returns(92736);
            yield return new TestCaseData(25).Returns(150050);
            yield return new TestCaseData(26).Returns(242786);
            yield return new TestCaseData(27).Returns(392836);
            yield return new TestCaseData(28).Returns(635622);
            yield return new TestCaseData(29).Returns(1028458);
            yield return new TestCaseData(30).Returns(1664080);
            yield return new TestCaseData(31).Returns(2692538);
            yield return new TestCaseData(32).Returns(4356618);
            yield return new TestCaseData(33).Returns(7049156);
            yield return new TestCaseData(34).Returns(11405774);
            yield return new TestCaseData(35).Returns(18454930);
            yield return new TestCaseData(36).Returns(29860704);
            yield return new TestCaseData(37).Returns(48315634);
            yield return new TestCaseData(38).Returns(78176338);
            yield return new TestCaseData(39).Returns(126491972);
            yield return new TestCaseData(40).Returns(204668310);
            yield return new TestCaseData(41).Returns(331160282);
            yield return new TestCaseData(42).Returns(535828592);
            yield return new TestCaseData(43).Returns(866988874);
            yield return new TestCaseData(44).Returns(1402817466);
            yield return new TestCaseData(45).Returns(2269806340);
        } 
    }
}
