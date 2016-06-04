using System;
using System.Collections.Generic;

namespace func_rocket
{
    public class Tasks
    {
        public static IEnumerable<GameSpace> GetGameSpaces()
        {
            var rocket = new Rocket(new Vector(50, 700), Vector.Zero, -0.25 * Math.PI);
            var target = new Vector(800, 300);

            yield return new GameSpace("zero-gravity", rocket, target, _ => Vector.Zero);
            yield return new GameSpace("heavy-gravity", rocket, target, _ => new Vector(Math.PI / 2) * 0.9);


            yield return new GameSpace("white-hole", rocket, target, point =>
            {
                var direction = point - target;
                var len = direction.Length;
                return direction.Normalize() * (50 * len / Math.Pow(len + 1, 2));
            });

            yield return new GameSpace("anomaly", rocket, target, point =>
            {
                const double sectorSize = (2 * Math.PI) / 1000;
                var sectorNumber = DateTime.Now.Millisecond % 1000;
                var angle = sectorNumber * sectorSize;
                return new Vector(angle);
            });

            yield return new GameSpace("mega-useless-tornado", rocket, target, point =>
            {
                var v = point - rocket.Location;
                var v2 = v.Rotate(Math.PI / 2);
                var dist = v.Length;
                return (v2 - v).Normalize() * (dist / 250);
            });
        }
        
        public static Turn TurboDoodleTechnique(Rocket rocket, Vector target)
        {
//            return Turn.None;
            var start = rocket.Location;
            var a = start + rocket.Velocity;
            var b = target;
            var v1 = a - start;
            var v2 = b - start;
            var det = v1.X * v2.Y - v1.Y * v2.X;
            var cw = det > 0;
            var ccw = det < 0;
            return cw ? Turn.Right : (ccw ? Turn.Left : Turn.None);
        }
    }
}
