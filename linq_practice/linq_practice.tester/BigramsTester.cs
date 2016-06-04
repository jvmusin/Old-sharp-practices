using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace linq_practice.tester
{
    [TestFixture]
    public class BigramsTester : AssertionHelper
    {
        public void TestBigrams<T>(IEnumerable<T> sequence, IEnumerable<Tuple<T, T>> bigrams)
        {
            Expect(sequence.GetBigrams(), EqualTo(bigrams));
        }

        [Test]
        public void Integers()
        {
            var sequence = new[] { 2, 6, 3, 1, 7 };
            var bigrams = new[]
            {
                Tuple.Create(2, 6),
                Tuple.Create(6, 3),
                Tuple.Create(3, 1),
                Tuple.Create(1, 7)
            };
            TestBigrams(sequence, bigrams);
        }

        [Test]
        public void OneElement()
        {
            TestBigrams(
                new object[1],
                Enumerable.Empty<Tuple<object, object>>());
        }

        [Test]
        public void EmptySequence()
        {
            TestBigrams(
                Enumerable.Empty<Tuple<object, object>>(),
                Enumerable.Empty<Tuple<object, object>>());
        }

        [Test]
        public void Nulls()
        {
            var sequence = new object[] { 1, null, 6, 7, null };
            var bigrams = new[]
            {
                Tuple.Create<object, object>(1, null),
                Tuple.Create<object, object>(null, 6),
                Tuple.Create<object, object>(6, 7),
                Tuple.Create<object, object>(7, null)
            };
            TestBigrams(sequence, bigrams);
        }

        [Test]
        public void SameElements()
        {
            var elem = new object();
            var sequence = new[] { elem, elem, elem };
            var bigrams = new[]
            {
                Tuple.Create(elem, elem),
                Tuple.Create(elem, elem)
            };
            TestBigrams(sequence, bigrams);
        }
    }
}
