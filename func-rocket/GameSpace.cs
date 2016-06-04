using System;
using System.Drawing;

namespace func_rocket
{
    public class GameSpace
    {
        public GameSpace(string name, Rocket rocket, Vector target, Func<Vector, Vector> gravity)
        {
            this.name = name;
            Rocket = rocket;
            Target = target;
            Gravity = gravity;
        }

        private readonly string name;
        public Rocket Rocket;
        public Vector Target;

        public override string ToString()
        {
            return name;
        }

        public Func<Vector, Vector> Gravity { get; }

        public void Move(Rectangle spaceRect, Turn turnRate)
        {
            Rocket.Direction += (int) turnRate*0.08;
            var direction = new Vector(Rocket.Direction); //  орт-вектор
            var force = direction + Gravity(Rocket.Location);
            Rocket.Velocity += force;
            if (Rocket.Velocity.Length > 20)
                Rocket.Velocity = 10*Rocket.Velocity.Normalize();
            Rocket.Location += Rocket.Velocity*0.5;

//            Âûðàâíèâàíèå ïî êðàÿì
            NormalizeLocation(ref Rocket.Location, spaceRect.Width, spaceRect.Height);
//            Rocket.Location = new Vector(Math.Min(spaceRect.Width, Rocket.Location.X),
//                Math.Min(spaceRect.Height, Rocket.Location.Y));
//            Rocket.Location = new Vector(Math.Max(0, Rocket.Location.X), Math.Max(0, Rocket.Location.Y));
        }

        private static void NormalizeLocation(ref Vector location, double maxX, double maxY)
        {
            var x = Math.Max(0, Math.Min(location.X, maxX));
            var y = Math.Max(0, Math.Min(location.Y, maxY));
            location = new Vector(x, y);
        }
    }
}
