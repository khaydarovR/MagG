using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace Mag.Common;

public static class DefaultRoles
{
    public const string rootConst = "Root пользователь";
    public const string adminConst = "Администратор";
    public const string userConst = "Пользователь";
    public const string unknownConst = "Требуется подтверждение";
    
    public static readonly IdentityRole Root = new IdentityRole() 
        { Name = rootConst };
    
    public static readonly IdentityRole Admin = new IdentityRole() 
        { Name = adminConst };
    
    public static readonly IdentityRole User = new IdentityRole() 
        { Name = userConst };
    
    public static readonly IdentityRole UnknownUser = new IdentityRole() 
        { Name = unknownConst };
    
    public static IdentityRole[] Roles = new[]
    {
        Root,
        Admin,
        User,
        UnknownUser
    };

    public static List<string> RolesString = new List<string>()
    {
        rootConst,
        adminConst,
        userConst,
        unknownConst
    };
}