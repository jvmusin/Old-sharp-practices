using System;

namespace hashes
{
	public class ReadonlyBytes
	{
		private readonly byte[] bytes;

		public ReadonlyBytes(byte[] bytes)
		{
			this.bytes = bytes;
		}
	}
}