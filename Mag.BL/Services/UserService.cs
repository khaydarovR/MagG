using System;
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
    private readonly SignInManager<AppUser> _signInManager;

    public UserService(
        UserManager<AppUser> userManager, 
        AppDbContext context, 
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _context = context;
        _signInManager = signInManager;
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
            UserName = user.UserName,
            UserId = user.Id
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

    public async Task<Response<UserEditVM>> SaveChange(UserEditVM model)
    {
        var dbUser = await _userManager.FindByIdAsync(model.UserId);
        var resultSetName = await _userManager.SetUserNameAsync(dbUser, model.UserName);

        var identityUserClaim = await _context.UserClaims
            .Where(t => t.ClaimType == ClaimTypes.Role)
            .SingleAsync(c => c.UserId == Guid.Parse(model.UserId));

        var oldClaim = new Claim(ClaimTypes.Role, identityUserClaim.ClaimValue);
        var newClaim = new Claim(ClaimTypes.Role, model.SelectedRole);
        var resultSetRole = await _userManager.ReplaceClaimAsync(dbUser, oldClaim, newClaim);

        if (resultSetRole.Succeeded && resultSetName.Succeeded)
        {
            //TODO: Обновить claims у пользователя при первом запросе(Detail?)
            return new Response<UserEditVM>(true);
        }

        return new Response<UserEditVM>("Ошибка при сохранении");
    }

    public async Task<Response<string>> DeleteUser(string guid)
    {
        var dbUser = await _userManager.Users.SingleAsync(u => u.Id == Guid.Parse(guid));
        var res = await _userManager.DeleteAsync(dbUser);

        if (res.Succeeded)
        {
            return new Response<string>(true);
        }

        return new Response<string>("Ошибка при удалении: " + res.Errors.ElementAt(0), false);
    }
}