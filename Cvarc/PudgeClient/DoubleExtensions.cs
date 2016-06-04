using System;

namespace PudgeClient
{
    public static class DoubleExtensions
    {
        public static int CompareTo(this double current, double other, double eps)
        {
            return Equals(current, other, eps) ? 0 : current.CompareTo(other);
        }

        public static bool Equals(this double current, double other, double eps)
        {
            return Math.Abs(current - other) < eps;
        }
    }
}
