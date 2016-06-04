using System;

namespace Tests
{
    public class GuidFactory : IGuidFactory
    {
        public Guid Create()
        {
            return Guid.NewGuid();
        }
    }
}