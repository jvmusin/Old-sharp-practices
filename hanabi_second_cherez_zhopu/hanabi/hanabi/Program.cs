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
            const string testName = "2-1";
            switch (Console.ReadLine())
            {
                case "0":
                    foreach (var result in MainRelease(testName))
                        Console.WriteLine(result);
                    break;
                default:
                    MainDebug(testName);
                    break;
            }
        }

        public static void MainDebug(string testName)
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

        public static IEnumerable<GameResult> MainRelease(string testName)
        {
            var hanabi = new HanabiInteractive();
            return File.ReadLines(@"C:\tests\" + testName + ".in", Encoding.UTF8)
                .Select(hanabi.MakeTurn)
                .Where(result => result != null);
        }
    }
}
