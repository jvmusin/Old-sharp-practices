using System;
using NUnit.Framework;

namespace Tests.Tests
{
    public class UserServiceTest : TestBase
    {
        private UserService userService;

        public override void SetUp()
        {
            userService = new UserService(new PasswordHasher(), new UserRepository(), new GuidFactory(), new UserEntityFactory());
        }

        [Test]
        public void TestRegisterAndLogin()
        {
            var login = "login";
            var password = "pass";

            userService.Register(login, password);
            var userModel = userService.Login(login, password);

            Assert.IsNotNull(userModel);
            Assert.AreEqual(userModel.Login, login);
        }

        [Test]
        public void TestRegisterAndLoginWithIncorrectPassword()
        {
            var login = "login";
            var password = "pass";
            var incorrectPass = "pass2";

            userService.Register(login, password);
            var userModel = userService.Login(login, incorrectPass);

            Assert.IsNull(userModel);
        }

        [Test]
        public void TestRegisterDuplicateUser()
        {
            var login = "login";
            var password = "pass";

            userService.Register(login, password);

            Assert.Throws<Exception>(() => userService.Register(login, password));
        }
    }
}