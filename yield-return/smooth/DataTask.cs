using System;
using System.Collections.Generic;

namespace yield
{
    public class DataPoint
    {
        public double X;
        public double OriginalY;
        public double ExpSmoothedY;
        public double AvgSmoothedY;
    }

    public static class DataTask
    {
        public static IEnumerable<DataPoint> GetData(Random random)
        {
            return GenerateOriginalData(random).SmoothExponentialy(0.2).MovingAverage(10);
        }

        public static IEnumerable<DataPoint> GenerateOriginalData(Random random)
        {
            for (var x = 0;; x++)
            {
                var y = 10*(1 - (x/50)%2) + 3*Math.Sin(x/20.0) + 2*random.NextDouble() - 1 + 3;
                yield return new DataPoint {X = x, OriginalY = y};
            }
        }

        public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
        {
            DataPoint prev = null;

            foreach (var point in data)
            {
                if (prev == null) prev = point;
                yield return new DataPoint
                {
                    AvgSmoothedY = point.AvgSmoothedY,
                    ExpSmoothedY = prev.ExpSmoothedY + alpha*(point.OriginalY - prev.ExpSmoothedY),
                    OriginalY = point.OriginalY,
                    X = point.X
                };
                prev = point;
            }
        }

        public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
        {
            var sum = 0.0;
            var values = new Queue<double>();
            foreach (var point in data)
            {
                values.Enqueue(point.OriginalY);
                sum += point.OriginalY;
                if (values.Count > windowWidth)
                    sum -= values.Dequeue();

                yield return new DataPoint
                {
                    AvgSmoothedY = sum/values.Count,
                    ExpSmoothedY = point.ExpSmoothedY,
                    OriginalY = point.OriginalY,
                    X = point.X
                };
            }
        }
    }
}