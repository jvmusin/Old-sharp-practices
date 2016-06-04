using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace hashes
{
	[TestClass]
	public class GhostKeyTest
	{
		[TestMethod]
		public void GhostKey_works_well()
		{
			var dictionary = new Dictionary<GhostKey, int>();
			var key1 = new GhostKey("Белая дама");
			var key2 = new GhostKey("Черная дама");
			var key3 = new GhostKey("Черная дама");
			
			dictionary[key1] = 42;
			dictionary[key2] = 43;
			dictionary[key3] = 44;
			
			Assert.AreEqual(42, dictionary[key1]);
			Assert.AreEqual(44, dictionary[key2]);
			Assert.AreEqual(2, dictionary.Count);
		}

		[TestMethod]
		public void GhostKey_dissapear_after_something()
		{
			var dictionary = new Dictionary<GhostKey, int>();
			var key = new GhostKey("Белая дама");
			dictionary.Add(key, 42);
			
			key.DoSomething();
			
			Assert.IsFalse(dictionary.ContainsKey(key));
		}
	}
}
