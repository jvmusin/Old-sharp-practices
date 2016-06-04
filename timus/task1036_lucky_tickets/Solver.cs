using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static helpers.Helpers;

namespace task1036_lucky_tickets
{
    public class Solver
    {
        public static void Main(string[] args)
        {
            var input = ReadInts().ToArray();
            var numberLength = input[0];
            var sum = input[1];
            Write(Solve(numberLength, sum));
        }

        public static BigInteger Solve(int numberLength, int sum)
        {
            if (sum % 2 != 0) return BigInteger.Zero;
            sum /= 2;

            var cache = new Dictionary<Tuple<int, int>, BigInteger>();
            var result = Solve(numberLength, sum, cache);
            return BigInteger.Pow(result, 2);
        }

        private static BigInteger Solve(int numberLength, int sum, IDictionary<Tuple<int, int>, BigInteger> calculated)
        {
            if (numberLength < 0 || sum < 0)
                return BigInteger.Zero;

            if (numberLength == 0 && sum == 0)
                return BigInteger.One;

            var cur = Tuple.Create(numberLength, sum);
            if (calculated.ContainsKey(cur))
                return calculated[cur];

            return calculated[cur] =
                Enumerable.Range(0, 10)
                    .Select(digit => Solve(numberLength - 1, sum - digit, calculated))
                    .Aggregate(BigInteger.Add);
        }
    }
}
