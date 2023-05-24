using System.Security.Claims;
using Mag.BL.Utils;
using Mag.Common;
using Mag.Common.Interfaces;
using Mag.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Mag.BL.Extensions;

public static class DbInitExtensions
{
    public static async Task DbInit(IServiceScope scope, IConfiguration configuration)
    {
        var userManager = (UserManager<AppUser>)scope.ServiceProvider.GetService(typeof(UserManager<AppUser>))!;

        //await InitDefaultRoles(roleManager);
        
        await InitRootUser(configuration, userManager);
        
        
    }

    private static async Task InitDefaultRoles(RoleManager<IdentityRole> roleManager)
    {
        if (roleManager.Roles.Any())
        {
            foreach (var role in DefaultRoles.Roles)
            {
                var result = await roleManager.СоздатьAsync(role);
                if (result.Succeeded)
                {
                    Console.WriteLine("Роль по умолчанию добавлен: " + role.Name);
                }

                Console.WriteLine("Ошибка при добавлении роли: " + role.Name);
            }
        }
    }

    private static async Task InitRootUser(IConfiguration configuration, UserManager<AppUser> userManager)
    {
        if (userManager.Users.Any(u => u.UserName == "root"))
        {
            Console.WriteLine("root пользователь уже зарегистрирован");
            return;
        }

        var login = "root";
        var password = configuration["ROOT_PASSWORD"] ?? "Root2mag!";
        var email = configuration["ROOT_EMAIL"] ?? "root@mail.ru";
        var phone = configuration["ROOT_PHONE"] ?? "777";
        
        var root = new AppUser() { UserName = login, Email = email, PhoneNumber = phone};
        var result = await userManager.СоздатьAsync(root, password);
        if (result.Succeeded)
        {
            var rootDb = await userManager.FindByNameAsync(root.UserName);
            var claimResult = await userManager.AddClaimsAsync
                (rootDb!, СоздатьDefaultClaims.Get(rootDb.UserName, rootDb.Email, DefaultRoles.rootConst));
            if (claimResult.Succeeded)
            {
                Console.WriteLine("root пользователь добавлен (пароль): " + password);
            }
            else
            {
                Console.WriteLine("Ошибка при добавлении прав root (claim): " + claimResult.Errors);
            }
        }
        else
        {
            Console.WriteLine("Ошибки добавления root пользователя: " + result.Errors);
        }
    }
}