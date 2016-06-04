using System.Collections.Generic;
using NUnit.Framework;

namespace hashes
{
	[TestFixture]
	public class ReadonlyBytesTests
	{

        [Test]
        public void Creation()
        {
            var data = new ReadonlyBytes(new byte[] { 1, 2, 3 });

            Assert.AreEqual(3, data.Length);
            Assert.AreEqual(1, data[0]);
        }

        [Test]
        public void Enumeration()
        {
            var data = new ReadonlyBytes(new byte[] { 1, 2, 3 });

            var list = new List<byte>();
            foreach (var x in data)
            {
                list.Add(x);
            }

            Assert.AreEqual(new byte[] { 1, 2, 3 }, list);
        }

        [Test]
        public void EqualityAndHashCode()
        {
            var data1 = new ReadonlyBytes(new byte[] { 1, 2, 3 });
            var data2 = new ReadonlyBytes(new byte[] { 1, 2, 3 });
            var data3 = new ReadonlyBytes(new byte[] { 1, 2, 3, 4 });

            Assert.AreEqual(data1.GetHashCode(), data2.GetHashCode());
            Assert.AreNotEqual(data1.GetHashCode(), data3.GetHashCode());
            Assert.AreEqual(data1, data2);
            Assert.AreNotEqual(data1, data3);
            Assert.AreNotEqual(data2, data3);
        }

    }
}