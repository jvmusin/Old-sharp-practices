using System.Collections.Generic;

namespace hashes
{
	public class SlowSequence
	{
	    public static IEnumerable<int> GetSlowSequence(int count)
	    {
	        for (var i = 0; i < count; i++)
	            yield return i*43627;
	    }
	}
}
