namespace Tests
{
    public interface IUserService
    {
        void Register(string login, string password);
        UserModel Login(string login, string password);
    }
}