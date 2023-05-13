using System.Security.Cryptography;

namespace Mag.Common;

public static class DefaultRoles
{
    public static readonly IdentityRole Root = new IdentityRole() 
        { Name = "Root пользователь со всеми правами", NormalizedName = "root" };
    
    public static readonly IdentityRole Admin = new IdentityRole() 
        { Name = "Администратор", NormalizedName = "admin" };
    
    public static readonly IdentityRole User = new IdentityRole() 
        { Name = "Пользователь", NormalizedName = "user" };
    
    public static readonly IdentityRole UnknownUser = new IdentityRole() 
        { Name = "Не подтвержденный пользователь", NormalizedName = "unknown" };
    
    public static IdentityRole[] Roles = new[]
    {
        Root,
        Admin,
        User,
        UnknownUser
    };
}

public record IdentityRole
{
    public string Name { get; init; }
    public string NormalizedName { get; init; }
}