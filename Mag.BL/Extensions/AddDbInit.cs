using Mag.Common;
using Mag.Common.Interfaces;
using Mag.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Mag.BL.Extensions;

public static class DbInitExtensions
{
    public static async Task DbInit(this IServiceScope scope, IConfiguration configuration)
    {
        var userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();
        if (userManager.Users.Any(u => u.UserName == "root"))
        {
            Console.WriteLine("root пользователь уже зарегистрирован");
            return;
        }
        var password = configuration["ROOT_PAS"]?? "Root2mag!";
        var login = "root";
        var email = configuration["ROOT_MAIL"] ?? "root@mail.ru";
        var root = new AppUser() { UserName = login, Email = email, Role = DefaultRoles.Root };
        var result = await userManager.CreateAsync(root, password);
        if (result.Succeeded)
        {
            Console.WriteLine("root пользователь добавлен (пароль): " + password);
        }
        else
        {
            Console.WriteLine("Ошибка добавления root пользователя");
        }
    }
}