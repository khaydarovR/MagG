using System.Security.Claims;
using Mag.BL.Utils;
using Mag.Common;
using Mag.Common.Models;
using Mag.Common.ViewModels;
using Mag.DAL;
using Mag.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mag.BL;

public class UserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _context;

    public UserService(
        UserManager<AppUser> userManager, 
        AppDbContext context, 
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _context = context;
    }


    public async Task<Response<UserVM>> GetAllUsers()
    {
        var dbUsers = _userManager.Users.ToList();

         var users = await MapToUserVM(dbUsers);

         return new Response<UserVM>(users);
    }

   

    public async Task<Response<UserEditVM>> GetUserForEdit(string guid)
    {
        var dbUser = await _userManager.Users.SingleAsync(u => u.Id == Guid.Parse(guid));
        var allRoles = await _context.UserClaims
            .Where(c => c.ClaimType == ClaimTypes.Role)
            .Select(t => t.ClaimValue)
            .ToListAsync();
        allRoles.AddRange(DefaultRoles.RolesString);
        allRoles = allRoles.Distinct().ToList();
        var user = await MapToUserView(dbUser);

        var userView = new UserEditVM()
        {
            Roles = allRoles,
            User = user
        };

        return new Response<UserEditVM>(userView);

    }
    
    
    private async Task<UserVM> MapToUserVM(List<AppUser> dbUsers)
    {
        UserVM userVM = new UserVM();
        foreach (var dbUser in dbUsers)
        {
            var userView = await MapToUserView(dbUser);

            userVM.Users.Add(userView);
        }
        return userVM;
    }

    private async Task<User> MapToUserView(AppUser dbUser)
    {
        var userView = new User()
        {
            CreatedDate = dbUser.CreatedDate.ToShortDateString(),
            Email = dbUser.Email,
            Id = dbUser.Id.ToString(),
            PhoneNumber = dbUser.PhoneNumber,
            UserName = dbUser.UserName,
            UserState = dbUser.UserState.ToString()
        };
        var claims = await _userManager.GetClaimsAsync(dbUser);
        userView.Role = claims.Single(c => c.Type == ClaimTypes.Role).Value;
        return userView;
    }
}