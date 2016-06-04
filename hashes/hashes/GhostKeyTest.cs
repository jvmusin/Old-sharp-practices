using System.Collections.Generic;
using NUnit.Framework;

namespace hashes
{
    [TestFixture]
    public class GhostKeyTest
    {
        [Test]
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

        [Test]
        public void GhostKey_dissapear_after_something()
        {
            var key1 = new GhostKey("Белая дама");
            var set = new HashSet<GhostKey> { key1 };

            key1.DoSomething();

            Assert.IsFalse(set.Contains(key1));
            Assert.AreEqual(1, set.Count);
            Assert.AreEqual("Белая дама", key1.Name);
        }
    }
}
