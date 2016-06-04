using System;
using System.Linq;
using Ninject;

namespace Tests
{
    public class Program
    {
        public static void Main()
        {
            var kernel = new StandardKernel(new UserServiceNinjectModule());

            var userService = kernel.Get<IUserService>();

            var login = "login";
            var password = "pass";
            userService.Register(login, password);
            var userModel = userService.Login(login, password);
            Console.WriteLine($"({userModel.Login}, {userModel.UserId})");

            Console.WriteLine(string.Join(", ", Enumerable.Range(0, 100).Where(i => i % 3 == 0)));
        }
    }
}
