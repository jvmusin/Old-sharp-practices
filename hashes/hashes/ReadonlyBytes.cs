using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable<byte>
    {
        private readonly byte[] bytes;
        public int Length => bytes.Length;

        public ReadonlyBytes(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentException(nameof(bytes));
            this.bytes = bytes;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return bytes.AsEnumerable().GetEnumerator();
        }

        public byte this[int i]
        {
            get { return bytes[i]; }
            set { bytes[i] = value; }
        }

        protected bool Equals(ReadonlyBytes other)
        {
            return Equals(bytes, other.bytes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ReadonlyBytes) obj);
        }

        public override int GetHashCode()
        {
            return bytes.Aggregate(0, (hash, curByte) => hash*31 + curByte);
        }
    }
}