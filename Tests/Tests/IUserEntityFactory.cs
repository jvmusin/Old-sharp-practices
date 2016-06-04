using System;

namespace Tests
{
    public interface IUserEntityFactory
    {
        UserEntity Create(string login, Guid userId, string passwordHash);
    }
}