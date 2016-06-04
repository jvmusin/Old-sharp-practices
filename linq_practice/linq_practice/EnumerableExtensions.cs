using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_practice
{
    public static class EnumerableExtensions
    {
        public static double? Median(this IEnumerable<double> items)
        {
            var sortedItems = items.OrderBy(x => x).ToArray();
            var len = sortedItems.Length;
            if (len == 0) return null;

            var mid = len/2;
            return (sortedItems[mid] + sortedItems[len - (mid + 1)])/2;
        }

        public static IEnumerable<Tuple<T, T>> GetBigrams<T>(this IEnumerable<T> items)
        {
            var previous = default(T);
            return items.Select(item =>
            {
                var result = Tuple.Create(previous, item);
                previous = item;
                return result;
            }).Skip(1);
        }
    }
}
