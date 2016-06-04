using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Pudge;

namespace PudgeClient
{
    public class Program
    {
        private const string CvarcTag = "Получи кварк-тэг на сайте";

        public static void Main(string[] args)
        {
            if (args.Length == 0)
                args = new[] { "127.0.0.1", "14000" };
            var ip = args[0];
            var port = int.Parse(args[1]);

            var trees = ReadTrees("trees.json");
            var client = new PudgeClientLevel2();
            var intellect = new PudgeIntellect(client, trees);

            var rnd = new Random();
            client.Configurate(ip, port, CvarcTag, seed:rnd.Next(), speedUp:true);
            intellect.Play();
            client.Exit();
        }

        private static IEnumerable<Point> ReadTrees(string fileName)
        {
            return JsonConvert.DeserializeObject<double[][]>(File.ReadAllText(fileName))
                .Select(position => new Point(position[0], position[1]));
        }
    }
}
