using System.Security.Claims;
using Mag.Common;
using Mag.Common.Interfaces;
using Mag.Common.Models;
using Mag.DAL;
using Mag.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Mag.BL;

public class UserService: IUserService<AppUser>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _context;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly EmailService _emailService;

    public UserService(
        UserManager<AppUser> userManager, 
        AppDbContext context, 
        SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        EmailService emailService)
    {
        _userManager = userManager;
        _context = context;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _emailService = emailService;
    }


    public async Task<Response<AppUser>> AddUser(UserRegisterDto model)
    {
        var newUser = await MapToAppUser(model);

        var createUser = await _userManager.CreateAsync(newUser, model.Password);
        
        if (createUser.Succeeded)
        {
            var dbUser = await _userManager.FindByEmailAsync(newUser.Email!);
            var addClaims = await _userManager.AddClaimsAsync(dbUser!, GetClaimsDefault(dbUser!));
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
            UserState = await _context.UserState.FirstAsync(s => s.State == StateEnum.Active),
            CreatedDate = DateTime.Today,
            EmailConfirmed = false,
            UserName = model.Email,
            SecurityStamp = DateTime.Now.ToLongTimeString()
        };
        return newUser ;
    }
    
    private List<Claim> GetClaimsDefault(AppUser newUser)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, newUser.Email, ClaimValueTypes.Integer),
            new Claim(ClaimTypes.Name, newUser.UserName),
            new Claim(ClaimTypes.Role, newUser.Role.Name),
        };

        return claims;
    }
}