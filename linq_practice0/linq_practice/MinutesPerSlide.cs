using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_practice
{
    public class MinutesPerSlide
    {
        private readonly SlideType[] slideTypes;
        private readonly Visit[] visits;

        public MinutesPerSlide(IEnumerable<SlideType> slideTypes, IEnumerable<Visit> visits)
        {
            this.slideTypes = slideTypes.Distinct().ToArray();
            this.visits = visits.OrderBy(visit => visit.Date).ToArray();
        }

        public MinutesPerSlide(IEnumerable<Slide> slides, IEnumerable<Visit> visits)
            : this(slides.Select(slide => slide.Type), visits)
        {
        }

        private Dictionary<SlideType, double> GetResult(Action<PeriodsHolder> periodsInitializer)
        {
            var periods = new PeriodsHolder(slideTypes);
            periodsInitializer(periods);
            return periods.PeriodsToMedian();
        }

        public Dictionary<SlideType, double> GetFast()
        {
            return GetResult(periods =>
            {
                var lastVisit = new Dictionary<User, Visit>();
                foreach (var visit in visits)
                {
                    if (lastVisit.ContainsKey(visit.User))
                        periods.TryAddPeriod(lastVisit[visit.User], visit);
                    lastVisit[visit.User] = visit;
                }
            });
        }

        public Dictionary<SlideType, double> GetUsingBigrams()
        {
            return GetResult(periods =>
            {
                var allUsers = visits.Select(visit => visit.User).Distinct();
                var allVisitPairs = allUsers.SelectMany(user => visits.Where(visit => visit.User.Equals(user)).GetBigrams());
                foreach (var twoVisits in allVisitPairs)
                    periods.TryAddPeriod(twoVisits.Item1, twoVisits.Item2);
            });
        }

        private class PeriodsHolder : Dictionary<SlideType, List<TimeSpan>>
        {
            public PeriodsHolder(IEnumerable<SlideType> slideTypes)
            {
                foreach (var slideType in slideTypes)
                    this[slideType] = new List<TimeSpan>();
            }

            public void TryAddPeriod(Visit previousVisit, Visit currentVisit)
            {
                var timeSpent = currentVisit - previousVisit;
                if (timeSpent.IsBetween(TimeSpan.FromMinutes(1), TimeSpan.FromHours(2)))
                    this[previousVisit.Slide.Type].Add(timeSpent);
            }

            public Dictionary<SlideType, double> PeriodsToMedian()
            {
                return this.ToDictionary(
                    typeAndPeriods => typeAndPeriods.Key,
                    typeAndPeriods => typeAndPeriods.Value.Select(period => period.TotalMinutes).Median() ?? 0);
            } 
        }
    }
}
