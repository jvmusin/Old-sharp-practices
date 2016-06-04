namespace Tests
{
    public interface IUserRepository
    {
        UserEntity Find(string login);
        void Create(UserEntity userEntity);
    }
}