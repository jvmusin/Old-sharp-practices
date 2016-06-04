using Ninject.Modules;

namespace Tests
{
    public class UserServiceNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPasswordHasher>().To<PasswordHasher2>();
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IGuidFactory>().To<GuidFactory>();
            Bind<IUserEntityFactory>().To<UserEntityFactory>();
            Bind<IUserService>().To<UserService>();
        }
    }
}
