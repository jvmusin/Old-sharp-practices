using NUnit.Framework;

namespace Tests.Tests
{
    public class PasswordHasherTest : TestBase
    {
        private PasswordHasher passwordHasher;

        public override void SetUp()
        {
            base.SetUp();
            passwordHasher = new PasswordHasher();
        }

        [Test]
        public void HashTest()
        {
            const string password = "pass";
            const string otherPassword = "pass2";
            var hash = passwordHasher.Hash(password);
            var sameHash = passwordHasher.Hash(password);
            var otherHash = passwordHasher.Hash(otherPassword);

            Assert.AreEqual(hash, sameHash);
            Assert.AreNotEqual(hash, otherHash);
        }
    }
}