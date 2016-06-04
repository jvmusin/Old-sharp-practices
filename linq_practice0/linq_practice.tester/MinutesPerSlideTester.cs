using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace linq_practice.tester
{
    [TestFixture]
    public class MinutesPerSlideTester : AssertionHelper
    {
        private static readonly Random Rnd = new Random();

        public void Test(IEnumerable<Slide> slides, IEnumerable<Visit> visits, Dictionary<SlideType, double> expected)
        {
            var solver = new MinutesPerSlide(slides, visits);
            var result = solver.GetFast();
            Expect(result, EquivalentTo(solver.GetUsingBigrams()));
            foreach (var typeAndMedian in result)
            {
                var type = typeAndMedian.Key;
                var median = typeAndMedian.Value;
                var expectedMedian = expected.ContainsKey(type) ? expected[type] : 0;
               
                Expect(DoubleEquals(median, expectedMedian));
            }
        }

        [Test]
        public void OneSlide()
        {
            var slide = new Slide("some slide", "some type", "some title");
            var user = new User("some user");

            const int delay = 10;
            var visits = Enumerable.Range(0, 20)
                .Select(i => new Visit(user, slide, DateTime.Now + TimeSpan.FromMinutes(delay*i)));

            Test(new[] {slide}, visits, new Dictionary<SlideType, double> {{slide.Type, delay}});
        }

        [Test]
        public void SomeSlidesWithSameType()
        {
            const string type = "some type";
            var slides = Enumerable.Range(0, 3).Select(i => new Slide("slide " + i, type, "title " + i)).ToArray();

            var user = new User("some user");

            const int delay = 10;
            var visits = Enumerable.Range(0, 20)
                .Select(i => new Visit(user, slides[i%slides.Length], DateTime.Now + TimeSpan.FromMinutes(delay*i)));

            Test(slides, visits, new Dictionary<SlideType, double> {{slides[0].Type, delay}});
        }

        [Test]
        public void ShuffledVisits()
        {
            var slide = new Slide("some slide", "some type", "some title");
            var user = new User("some user");
            
            const int delay = 10;
            var visits = Enumerable.Range(0, 20)
                .Select(i => new Visit(user, slide, DateTime.Now + TimeSpan.FromMinutes(delay*i)))
                .OrderBy(_ => Rnd.Next());

            Test(new[] {slide}, visits, new Dictionary<SlideType, double> {{slide.Type, delay}});
        }

        [Test]
        public void SomeUsers()
        {
            var slide = new Slide("some slide", "some type", "some title");
            var users = Enumerable.Range(0, 3).Select(i => new User("user " + i)).ToArray();

            const int delay = 10;
            var visits = Enumerable.Range(0, 20)
                .Select(i => new Visit(users[i%users.Length], slide, DateTime.Now + TimeSpan.FromMinutes(delay*i)));

            Test(new[] {slide}, visits, new Dictionary<SlideType, double> {{slide.Type, delay*users.Length}});
        }

        [Test]
        public void LargeAndLittlePeriods()
        {
            var slide = new Slide("some slide", "some type", "some title");
            var user = new User("some user");

            var visits = new List<Visit>();
            for (var i = 0; i < 20; i++)
            {
                var previousTime = i == 0 ? DateTime.Now : visits.Last().Date;
                var delayInSeconds = Rnd.Next(2) == 0 ? (Rnd.Next(60)) : (Rnd.Next(100500) + 2*60*60 + 1);
                var nextTime = previousTime + TimeSpan.FromSeconds(delayInSeconds);
                visits.Add(new Visit(user, slide, nextTime));
            }

            Test(new [] {slide}, visits, new Dictionary<SlideType, double>());
        }

        private static bool DoubleEquals(double value1, double value2)
        {
            const double eps = 1e-6;
            return Math.Abs(value1 - value2) < eps;
        }
    }
}
