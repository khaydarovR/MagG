using System.Security.Claims;
using Mag.BL.Utils;
using Mag.Common;
using Mag.Common.Interfaces;
using Mag.Common.Models;
using Mag.DAL;
using Mag.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace Mag.BL;

public class AccountService: IAccountService<AppUser>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountService(
        UserManager<AppUser> userManager, 
        AppDbContext context, 
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }


    public async Task<Response<AppUser>> AddUser(UserRegisterDto model)
    {
        var newUser = await MapToAppUser(model);

        var createUser = await _userManager.CreateAsync(newUser, model.Password);
        
        if (createUser.Succeeded)
        {
            var dbUser = await _userManager.FindByEmailAsync(newUser.Email!);
            var addClaims = await _userManager
                .AddClaimsAsync(dbUser!, CreateDefaultClaims.Get(dbUser.Email, dbUser.UserName));
            if (addClaims.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, true);
                return new Response<AppUser>(dbUser!);
            }
        }

        var response = new Response<AppUser>(false);
        foreach (var error in createUser.Errors)
        {
            response.Errors.Add(error.Description);
        }
        return response;
    }

    public async Task<Response<AppUser>> LoginUser(UserLoginDto model)
    {
        var result = await _signInManager.PasswordSignInAsync
            (model.UserName, model.Password, model.RememberMe, false);
        var userDb = await _userManager.FindByNameAsync(model.UserName);
        if (result.Succeeded && userDb is not null)
        {
            return new Response<AppUser>(userDb);
        }

        return new Response<AppUser>("Ошибка авторизации");
    }

    public async Task<Response<AppUser>> GetUserDetail(ClaimsPrincipal claimsPrincipal)
    {
        var result = await _userManager.GetUserAsync(claimsPrincipal);
        
        if (result is not null)
        {
            return new Response<AppUser>(result);
        }

        await _signInManager.SignOutAsync();
        return new Response<AppUser>("Ошибка в куках, войдите заного", false);
    }
    
    
    public async Task<Response<AppUser>> ExitFromAccount()
    {
        await _signInManager.SignOutAsync();
        return new Response<AppUser>(true);
    }

    public async Task<Response<AppUser>> Edit(UserEditDto userEditDto, ClaimsPrincipal claimsPrincipal)
    {
        var userDb = await _userManager.GetUserAsync(claimsPrincipal);

        if (userDb is null)
        {
            return new Response<AppUser>("Пользователь не найден");
        }
        var userName = await _userManager.SetUserNameAsync(userDb, userEditDto.UserName);
        var password = await _userManager.ChangePasswordAsync(
            userDb, userEditDto.PasswordOld, userEditDto.PasswordNew);
        

        if (userName.Succeeded && password.Succeeded)
        {
            return new Response<AppUser>(true);
        }
        
        var response = new Response<AppUser>(false);
        foreach (var error in userName.Errors)
        {
            response.Errors.Add(error.Description);
        }
        foreach (var error in password.Errors)
        {
            response.Errors.Add(error.Description);
        }
        return response;
    }


    private async Task<AppUser> MapToAppUser(UserRegisterDto model)
    {
        var newUser = new AppUser()
        {
            Email = model.Email,
            PhoneNumber = model.Phone,
            UserState = StateEnum.Active,
            CreatedDate = DateTime.Today,
            EmailConfirmed = false,
            UserName = model.Email,
            SecurityStamp = DateTime.Now.ToLongTimeString()
        };
        return newUser ;
    }
}