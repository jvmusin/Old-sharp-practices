namespace Tests
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}