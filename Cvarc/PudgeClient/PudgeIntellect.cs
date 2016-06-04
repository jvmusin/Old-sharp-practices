using System;
using System.Collections.Generic;
using System.Linq;
using CVARC.Core.Controllers.Network;
using CVARC.V2;
using Pudge;
using Pudge.Player;

using static PudgeClient.GeometryUtils;

namespace PudgeClient
{
    public class PudgeIntellect
    {
        private PudgeClient<PudgeSensorsData> Client { get; }
        private PudgeSensorsData data;
        private Waypoint[] Graph { get; }
        private Dictionary<Point, int> RunesUsedCount { get; }
        private Point[] Trees { get; }

        public PudgeIntellect(PudgeClient<PudgeSensorsData> client, IEnumerable<Point> trees)
        {
            Client = client;
            Client.SensorDataReceived += data => this.data = data;
            Client.SensorDataReceived += PrintData;

            Trees = trees.ToArray();
            Graph = BuildWaypointsGraph().ToArray();
            RunesUsedCount = new Dictionary<Point, int>();
            InitRunesUsedCount();
        }

        public void Play()
        {
            try
            {
                Wait();

                while (true)
                {
                    if (!data.IsDead)
                        Move(FindPathToNearestRune());
                    else Wait();
                }
            }
            catch (KickOutException)
            {
                Console.WriteLine("Kicked");
            }
        }

        #region Move methods

        private bool Move(Point destination, int iterationsLimit = int.MaxValue)
        {
            var iterationsDone = 0;
            while (!data.IsDead && ++iterationsDone < iterationsLimit)
            {
                TryKillSomeone();
                var self = data.SelfLocation;

                var distance = (self - destination).RadiusVectorLength;
                if (distance <= 3) return true;

                var angle = GetAngle(self, destination);
                if (Math.Abs(angle) >= 3)
                    Rotate(angle);
                Move(1);
            }
            return false;
        }

        private void Move(double x, double y)
        {
            Move(new Point(x, y));
        }
        
        private void Move(IEnumerable<Point> path)
        {
            foreach (var position in path)
                if (!Move(position, 5))
                    break;
        }

        private void Move(double distance = 10)
        {
            Client.Move(distance);
        }

        #endregion

        private double lastTimeHooked = -100;
        private void TryKillSomeone()
        {
            var heroes = data.Map.Heroes;
            if (heroes.Any() && data.WorldTime - lastTimeHooked >= PudgeRules.Current.HookCooldown)
            {
                var heroLocation = new Point(heroes.First().Location);
                Hook(heroLocation);
            }
        }

        private void Rotate(double angle)
        {
            Client.Rotate(angle);
        }

        private void Wait(double waitingTime = 0)
        {
            Client.Wait(waitingTime);
        }

        private void Hook(Point target)
        {
            Rotate(GetAngle(data.SelfLocation, target));
            Client.Hook();
            lastTimeHooked = data.WorldTime;
        }

        private static double GetAngle(LocatorItem self, Point destination)
        {
            var selfDirection = self.Angle;
            var needDirection = (destination - new Point(self)).Angle;
            if (needDirection < selfDirection) needDirection += 360;
            return From360To180Degrees(needDirection - selfDirection);
        }

        public static void PrintData(PudgeSensorsData data)
        {
            Console.WriteLine("---------------------------------");
            if (data.IsDead)
            {
                Console.WriteLine("Ooops, i'm dead :(");
                return;
            }
            Console.WriteLine("I'm here: " + data.SelfLocation);
            Console.WriteLine("My score now: {0}", data.SelfScores);
            Console.WriteLine("Current time: {0:F}", data.WorldTime);
            foreach (var rune in data.Map.Runes)
                Console.WriteLine("Rune! Type: {0}, Size = {1}, Location: {2}", rune.Type, rune.Size, rune.Location);
            foreach (var heroData in data.Map.Heroes)
                Console.WriteLine("Enemy! Type: {0}, Location: {1}, Angle: {2:F}", heroData.Type, heroData.Location,
                    heroData.Angle);
            foreach (var eventData in data.Events)
                Console.WriteLine("I'm under effect: {0}, Duration: {1}", eventData.Event,
                    eventData.Duration - (data.WorldTime - eventData.Start));
            Console.WriteLine("---------------------------------");
            Console.WriteLine();
        }

        private IEnumerable<Waypoint> BuildWaypointsGraph()
        {
            const int minPosition = -140;
            const int maxPosition = 140;
            const int range = maxPosition - minPosition + 1;
            
            Predicate<int> isInRange = position =>
                minPosition <= position && position <= maxPosition;
            Func<int, int> normalize = index => index - minPosition;


            var graph = new Waypoint[range, range];
            for (var x = minPosition; x <= maxPosition; x++)
                for (var y = minPosition; y <= maxPosition; y++)
                {
                    var current = graph[normalize(x), normalize(y)] = new Waypoint(x, y);
                    if (!CanBeVisited(current))
                    {
                        current.MarkAsUnavailable();
                        continue;
                    }

                    for (var deltaX = -1; deltaX <= 0; deltaX++)
                        for (var deltaY = -1; deltaY <= +1; deltaY++)
                        {
                            var nextX = x + deltaX;
                            var nextY = y + deltaY;

                            if (!isInRange(nextX) || !isInRange(nextY))
                                continue;
                            var next = graph[normalize(nextX), normalize(nextY)];
                            if (next == null || next == current || !next.Available)
                                continue;

                            current.Connect(next);
                        }
                }
            
            return graph.Cast<Waypoint>().Where(waypoint => waypoint.Available);
        }

        private bool CanBeVisited(Point position)
        {
            return !Trees.Any(tree => tree.DistanceTo(position) < 25);
        }

        private void InitRunesUsedCount()
        {
            RunesUsedCount[new Point(0, 0)] = 0;

            RunesUsedCount[new Point(-120, -70)] = 0;
            RunesUsedCount[new Point(70, 120)] = 0;
            RunesUsedCount[new Point(120, 70)] = 0;
            RunesUsedCount[new Point(-70, -120)] = 0;

            RunesUsedCount[new Point(-130, 130)] = 0;
            RunesUsedCount[new Point(130, -130)] = 0;
        }

        private IEnumerable<Point> FindPathToNearestRune()
        {
            var startX = (int) data.SelfLocation.X;
            var startY = (int) data.SelfLocation.Y;
            var startPoint = new Point(startX, startY);
            var start = Graph.OrderBy(waypoint => startPoint.DistanceTo(waypoint)).First();

            var totalDistance = new Dictionary<Point, double>();
            var queue = new SortedSet<Tuple<double, Waypoint>>();
            var parent = new Dictionary<Point, Point>();

            totalDistance[start] = 0.0;
            queue.Add(Tuple.Create(0.0, start));

            var target = new Point(0, 0);
            while (queue.Any())
            {
                var currentDistance = queue.First().Item1;
                var currentWaypoint = queue.First().Item2;
                queue.Remove(queue.First());

                if (currentDistance != totalDistance[currentWaypoint])
                    continue;

                if (IsRune(currentWaypoint) &&
                    RunesUsedCount[currentWaypoint] <= (int) data.WorldTime / 22)
                {
                    RunesUsedCount[currentWaypoint]++;
                    target = currentWaypoint;
                    break;
                }

                foreach (var nextWaypoint in currentWaypoint.ConnectedWaypoints)
                {
                    if (!totalDistance.ContainsKey(nextWaypoint))
                        totalDistance[nextWaypoint] = double.MaxValue;

                    var oldDistance = totalDistance[nextWaypoint];
                    var newDistance = currentDistance + currentWaypoint.DistanceTo(nextWaypoint);
                    if (oldDistance.CompareTo(newDistance, 1e-6) > 0)
                    {
                        totalDistance[nextWaypoint] = newDistance;
                        queue.Add(Tuple.Create(newDistance, nextWaypoint));
                        parent[nextWaypoint] = currentWaypoint;
                    }
                }
            }
            
            var path = new List<Point>();
            for (var position = target; position != null; position = parent.SafeGet(position))
                path.Add(position);
            path.Reverse();

            if (!path.Any()) Wait();
            return path;
        }

        private bool IsRune(Point position)
        {
            return RunesUsedCount.ContainsKey(position);
        }
    }
}
