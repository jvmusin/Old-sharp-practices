using System;

namespace linq_practice
{
    public static class TimeSpanExtensions
    {
        public static bool IsBetween(this TimeSpan span, TimeSpan firstBorder, TimeSpan secondBorder)
        {
            if (firstBorder > secondBorder)
                return IsBetween(span, secondBorder, firstBorder);
            return firstBorder <= span && span <= secondBorder;
        }
    }
}
