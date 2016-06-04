namespace hashes
{
	public class FastKey
	{
		public int X { get; private set; }
		public int Y { get; private set; }

		public FastKey(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}