using NUnit.Framework;
using Rhino.Mocks;

namespace Tests.Tests
{
    public class TestBase
    {
        protected MockRepository mockRepository;

        [SetUp]
        public virtual void SetUp()
        {
            mockRepository = new MockRepository();
        }

        protected T NewMock<T>()
        {
            return mockRepository.StrictMock<T>();
        }
    }
}