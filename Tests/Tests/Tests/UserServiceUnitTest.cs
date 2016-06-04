using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests.Tests
{
    public class UserServiceUnitTest : TestBase
    {
        private IPasswordHasher passwordHasher;
        private UserService userService;
        private IUserRepository userRepository;
        private IGuidFactory guidFactory;
        private IUserEntityFactory userEntityFactory;

        public override void SetUp()
        {
            base.SetUp();

            passwordHasher = NewMock<IPasswordHasher>();
            userRepository = NewMock<IUserRepository>();
            guidFactory = NewMock<IGuidFactory>();
            userEntityFactory = NewMock<IUserEntityFactory>();
            userService = new UserService(passwordHasher, userRepository, guidFactory, userEntityFactory);
        }

        [Test]
        public void TestRegister()
        {
            var login = "login";
            var password = "pass";
            var hash = "passHash";
            var userId = Guid.NewGuid();
            var userEntity = new UserEntity();

            using (mockRepository.Record())
            {
                passwordHasher.Expect(f => f.Hash(password)).Return(hash);
                guidFactory.Expect(f => f.Create()).Return(userId);
                userEntityFactory.Expect(f => f.Create(login, userId, hash)).Return(userEntity);
                userRepository.Expect(f => f.Create(userEntity));
            }

            userService.Register(login, password);
        }

        [Test]
        public void TestLogin()
        {
            var login = "login";
            var password = "pass";
            var hash = "passHash";
            var userId = Guid.NewGuid();
            var userEntity = new UserEntity
            {
                Login = login,
                PasswordHash = hash,
                UserId = userId
            };

            using (mockRepository.Record())
            {
                userRepository.Expect(f => f.Find(login)).Return(userEntity);
                passwordHasher.Expect(f => f.Hash(password)).Return(hash);
            }

            var userModel = userService.Login(login, password);

            Assert.IsNotNull(userModel);
            Assert.AreEqual(login, userModel.Login);
            Assert.AreEqual(userId, userModel.UserId);
        }

        [Test]
        public void TestLoginUnexistedUser()
        {
            var login = "login";
            var password = "pass";

            using (mockRepository.Record())
            {
                userRepository.Expect(f => f.Find(login)).Return(null);
            }

            var userModel = userService.Login(login, password);
            
            Assert.IsNull(userModel);
        }
    }
}