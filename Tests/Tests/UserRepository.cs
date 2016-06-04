using System;
using System.Collections.Concurrent;

namespace Tests
{
    public class UserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<string, UserEntity> users = new ConcurrentDictionary<string, UserEntity>();

        public UserEntity Find(string login)
        {
            UserEntity entity;
            return !users.TryGetValue(login, out entity)
                ? null
                : entity;
        }

        public void Create(UserEntity userEntity)
        {
            if (!users.TryAdd(userEntity.Login, userEntity))
                throw new Exception("Fck");
        }
    }
}