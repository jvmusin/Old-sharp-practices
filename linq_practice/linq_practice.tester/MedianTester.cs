using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace linq_practice.tester
{
    [TestFixture]
    public class MedianTester : AssertionHelper
    {
        private static readonly Random Rnd = new Random();
        public void TestMedian(IEnumerable<double> sequence, double? median)
        {
            Expect(sequence.Median(), EqualTo(median));
        }

        [Test]
        public void EvenLen()
        {
            TestMedian(
                new[] {-5, 4, 456.23, 2222.55, 1000000.9, 2e90},
                (456.23 + 2222.55)/2);
        }

        [Test]
        public void NotEvenLen()
        {
            TestMedian(
                new[] {-5, -1, 4, 456.23, 2222.55, 1000000.9, 2e90},
                456.23);
        }

        [Test]
        public void Shuffled()
        {
            TestMedian(
                new[] {-5, -1, 4, 456.23, 2222.55, 1000000.9, 2e90}.OrderBy(x => Rnd.Next()),
                456.23);
        }

        [Test]
        public void EmptySequence()
        {
            TestMedian(new double[0], null);
        }
    }
}
