using static helpers.Helpers;

namespace task1225_flags
{
    public class Solver
    {
        public static void Main(string[] args)
        {
            Write(Solve(ReadInt()));
        }

        public static long Solve(int stripeCount)
        {
            var a = new long[stripeCount + 1, 2];
            a[1, 0] = 2;
            for (var i = 2; i <= stripeCount; i++)
            {
                a[i, 0] = a[i - 1, 0] + a[i - 1, 1];
                a[i, 1] = a[i - 1, 0];
            }
            return a[stripeCount, 0];
        }
    }
}
