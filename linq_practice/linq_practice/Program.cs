using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace linq_practice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var slides = File.ReadLines("slides.txt", Encoding.Default)
                .Skip(1)
                .Select(Slide.Parse)
                .ToDictionary(slide => slide.Id);

            var users = new Dictionary<string, User>();
            var visits = File.ReadLines("visits.txt", Encoding.Default)
                .Skip(1)
                .Select(line => Visit.Parse(line, slides, users))
                .ToList();
            
            PrintResults(slides.Values, visits);
            new ByDaysOfWeekCounterHistogram(visits).ShowDialog();
        }

        private static void PrintResults(IEnumerable<Slide> slides, IEnumerable<Visit> visits)
        {
            var solvers = GetSolvers(new MinutesPerSlide(slides, visits));
            foreach (var solver in solvers)
            {
                Console.WriteLine($"Method: {solver.Method.Name}");

                var start = Stopwatch.StartNew();
                var result = string.Join("\n", solver().Select(ResultToString));
                var elapsedTime = start.Elapsed;

                Console.WriteLine($"Elapsed time: {elapsedTime.Milliseconds} (millis)");
                Console.WriteLine("Result:");
                Console.WriteLine(result);
                Console.WriteLine("\n".PadLeft(30, '-'));
            }
        }

        private static IEnumerable<Func<Dictionary<SlideType, double>>> GetSolvers(MinutesPerSlide solverObject)
        {
            return new Func<Dictionary<SlideType, double>>[]
            {
                solverObject.GetFast,
                solverObject.GetUsingBigrams
            };
        }

        private static string ResultToString(KeyValuePair<SlideType, double> result)
        {
            return $"{result.Value:F2} minutes per {result.Key} slide";
        }
    }
}
