using System.Collections.Generic;

namespace hanabi
{
    public static class EnumerableExtensions
    {
        public static string Join<T>(this IEnumerable<T> sequence, string separator = " ")
        {
            return string.Join(separator, sequence);
        }
    }
}
