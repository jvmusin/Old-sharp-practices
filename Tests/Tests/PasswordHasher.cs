using System;

namespace Tests
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            var hash = new char[password.Length];
            for (var i = 0; i < password.Length; ++i)
            {
                hash[i] = (char)(password[i] + 5);
            }
            return new string(hash);
        }
    }
    public class PasswordHasher2 : IPasswordHasher
    {
        public string Hash(string password)
        {
            var hash = new char[password.Length];
            for (var i = 0; i < password.Length; ++i)
            {
                hash[i] = (char)(password[i] + 5);
            }
            return new string(hash);
        }
    }
}