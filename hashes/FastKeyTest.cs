using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace hashes
{
	[TestClass]
	public class FastKeyTest
	{
		private Dictionary<FastKey, FastKey> dictionary;

		[TestInitialize]
		public void Init()
		{
			dictionary = new Dictionary<FastKey, FastKey>();
		}

		public void Add(FastKey key)
		{
			dictionary[key] = key;
		}

		[TestMethod]
		public void FastKey_should_be_correct()
		{
			var d = new Dictionary<FastKey, int>();
			d.Add(new FastKey(1, 1), 1);
			d.Add(new FastKey(1, 2), 2);
			d.Add(new FastKey(2, 1), 3);
			Assert.AreEqual(1, d[new FastKey(1, 1)]);
			Assert.AreEqual(2, d[new FastKey(1, 2)]);
			Assert.AreEqual(3, d[new FastKey(2, 1)]);
			Assert.AreEqual(3, d.Count);
			d[new FastKey(1, 1)] = 100500;
			Assert.AreEqual(100500, d[new FastKey(1, 1)]);
			Assert.AreEqual(3, d.Count);
		}

		[TestMethod]
		public void FastKey_work_well_with_large_values()
		{
			Add(new FastKey(int.MaxValue, int.MaxValue));
			Add(new FastKey(int.MaxValue, int.MinValue));
			Add(new FastKey(int.MinValue, int.MaxValue));
			Add(new FastKey(int.MinValue, int.MinValue));

			Assert.AreEqual(4, dictionary.Count);
		}


		[TestMethod]
		public void FastKey_should_be_fast()
		{
			var sw = Stopwatch.StartNew();
			AddSecretKeysToDictionary();

			foreach (var pair in dictionary)
			{
				Assert.AreEqual(pair.Value, pair.Key);
				Assert.AreEqual(pair.Value.X, pair.Key.X);
				Assert.AreEqual(pair.Value.Y, pair.Key.Y);
			}

			Assert.IsTrue(sw.ElapsedMilliseconds < 2000, "should be fast!");
		}

		private void AddSecretKeysToDictionary()
		{
			const int range = 200000;
			var rnd = new Random();
			for (int x = -range; x < range; x++)
			{
				var multiplicator = rnd.Next(-7, 8);
				Add(new FastKey(x, multiplicator * x));
				Add(new FastKey(x * multiplicator, x));
			}
			for (int x = 10*range; x < 10*range + 25000; x++)
			{
				Add(new FastKey(x, 0));
				Add(new FastKey(x, x * 397));
				Add(new FastKey(x, -x * 1023));
				Add(new FastKey(x, -x * 1039));
				Add(new FastKey(0, x));
				Add(new FastKey(x * 397, x));
				Add(new FastKey(-x * 1023, x));
				Add(new FastKey(-x * 1039, x));
			}

			Assert.IsTrue(dictionary.Count >= range * 2);
		}
	}
}
