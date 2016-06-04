using System;

namespace Tests
{
    public class UserEntity
    {
        public string Login { get; set; }
        public Guid UserId { get; set; }
        public string PasswordHash { get; set; }
    }
}