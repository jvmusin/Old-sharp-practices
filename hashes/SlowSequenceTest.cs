using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace hashes
{
	[TestClass]
	public class SlowSequenceTest
	{
		[TestMethod]
		public void Dictionary_of_int_work_slowly_on_special_sequence()
		{
			const int count = 40000;
			var ordinaryTime = CheckSequence(Enumerable.Range(0, int.MaxValue), count);
			var badTime = CheckSequence(SlowSequence.GetSlowSequence(count), count);
			Assert.IsTrue(ordinaryTime * 100 < badTime, "should be ~1000 times slower than ordinary sequence!");
		}

		private static long CheckSequence(IEnumerable<int> sequence, int count)
		{
			var list = sequence.Take(count).ToList();
			Assert.AreEqual(count, list.Count, "Sequence must contain at least " + count + " elements");
			var stopwatch = Stopwatch.StartNew();
			var dictionary = new Dictionary<int, int>(list.Count);
			foreach (var n in list)
				dictionary[n] = n;
			foreach (var n in list)
				Assert.AreEqual(n, dictionary[n]);
			stopwatch.Stop();
			Assert.AreEqual(list.Count, dictionary.Count);
			Console.WriteLine(stopwatch.Elapsed);
			Console.WriteLine("{0:#} operations per second", list.Count / stopwatch.Elapsed.TotalSeconds);
			Console.WriteLine();
			return stopwatch.ElapsedMilliseconds;
		}
	}


}
