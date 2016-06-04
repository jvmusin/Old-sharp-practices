using System.Collections.Generic;
using System.Linq;

namespace hashes
{
	public class SlowSequence
	{
		public static IEnumerable<int> GetSlowSequence(int count)
		{
			yield return 42;
		}
		
	}
}