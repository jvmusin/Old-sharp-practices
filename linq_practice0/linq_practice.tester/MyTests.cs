using System.Collections;
using NUnit.Framework;

namespace linq_practice.tester
{
    [TestFixture]
    public class MyTests : AssertionHelper
    {
        [Test]
        [TestCaseSource(typeof(MyDataClass), nameof(MyDataClass.TestCases))]
        public int DivideTest(int n, int d)
        {
            return n / d;
        }

        [Test]
        public void DivideTest1()
        {
            Expect(6, EqualTo(2*3));
        }
    }

    public class MyDataClass
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(12, 3).Returns(4);
                yield return new TestCaseData(12, 2).Returns(6);
                yield return new TestCaseData(12, 4).Returns(3);
            }
        }
    }
}
