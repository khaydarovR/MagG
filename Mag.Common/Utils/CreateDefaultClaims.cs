using System.Security.Claims;
using Mag.Common;

namespace Mag.BL.Utils;

public static class CreateDefaultClaims
{
    public static List<Claim> Get(string email, string userName, string role = DefaultRoles.unknownConst)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, role),
        };

        return claims;
    }
}