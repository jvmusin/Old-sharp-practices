namespace func_rocket
{
	public class Rocket
	{
		public Rocket(Vector location, Vector velocity, double direction)
		{
			Location = location;
			Velocity = velocity;
			Direction = direction;
		}

		public Vector Location;
		public Vector Velocity;
		public double Direction;
	}
}