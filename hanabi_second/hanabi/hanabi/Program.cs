using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace hanabi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string testName = "test0";
            switch (Console.ReadLine())
            {
                case "0":
                    foreach (var result in ResultsOnlyMain(testName))
                        Console.WriteLine(result);
                    break;
                default:
                    DebugMain(testName);
                    break;
            }
//            ReleaseMain();
        }

        public static void ReleaseMain()
        {
            var hanabi = new HanabiInteractive();
            var gameResults = ReadLines().Select(hanabi.MakeTurn).Where(result => result != null);
            foreach (var result in gameResults)
                Console.WriteLine(result);
            Console.Out.Flush();
        }

        private static IEnumerable<string> ReadLines()
        {
            string s;
            while ((s = Console.ReadLine()) != null)
                yield return s;
        }

#region sss
        public static void DebugMain(string testName)
        {
            var hanabi = new HanabiInteractive {Debug = true};
            foreach (var command in File.ReadLines(@"C:\tests\" + testName + ".in", Encoding.UTF8))
            {
                Console.WriteLine(command + "\n");
                var result = hanabi.MakeTurn(command);
                if (result != null)
                    Console.WriteLine($"\n{result}    {result.Cause.Message}");
                Console.WriteLine("\n----------------------------------\n");
                Console.ReadKey();
            }
        }
#endregion

        public static IEnumerable<GameResult> ResultsOnlyMain(string testName)
        {
            var hanabi = new HanabiInteractive();
            return File.ReadLines(@"C:\tests\" + testName + ".in", Encoding.UTF8)
                .Select(hanabi.MakeTurn)
                .Where(result => result != null);
        }
    }
}