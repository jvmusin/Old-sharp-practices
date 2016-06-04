namespace Tests
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher passwordHasher;
        private readonly IUserRepository userRepository;
        private readonly IGuidFactory guidFactory;
        private readonly IUserEntityFactory userEntityFactory;

        public UserService(IPasswordHasher passwordHasher, IUserRepository userRepository,
            IGuidFactory guidFactory, IUserEntityFactory userEntityFactory)
        {
            this.passwordHasher = passwordHasher;
            this.userRepository = userRepository;
            this.guidFactory = guidFactory;
            this.userEntityFactory = userEntityFactory;
        }

        public void Register(string login, string password)
        {
            var passwordHash = passwordHasher.Hash(password);
            var userId = guidFactory.Create();
            var userEntity = userEntityFactory.Create(login, userId, passwordHash);

            userRepository.Create(userEntity);
        }

        public UserModel Login(string login, string password)
        {
            var entity = userRepository.Find(login);

            if (entity == null)
                return null;

            var hash = passwordHasher.Hash(password);
            if (hash != entity.PasswordHash)
                return null;

            return new UserModel
            {
                Login = entity.Login,
                UserId = entity.UserId
            };
        }
    }
}