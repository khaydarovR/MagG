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


    public async Task<Response<string>> AddUser(UserRegisterDto model)
    {
        var newUser = await MapToAppUser(model);

        var result = await _userManager.CreateAsync(newUser, model.Password);
        
        if (result.Succeeded)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            await _userManager.AddToRoleAsync(newUser, DefaultRoles.UnknownUser.NormalizedName);
            await _signInManager.SignInAsync(newUser, true);
            return new Response<string>(true){ResponseModel = code};
        }

        var response = new Response<string>(false);
        foreach (var error in result.Errors)
        {
            response.Errors.Add(error.Description);
        }
        return response;
    }

    public async Task<Response<AppUser>> LoginUser(UserLoginDto model)
    {
        var result = await _signInManager.PasswordSignInAsync
            (model.Email, model.Password, model.RememberMe, false);
        
        if (result.Succeeded)
        {
            return new Response<AppUser>(await _userManager.Users.FirstAsync(u => u.Email == model.Email), true);
        }

        return new Response<AppUser>("Ошибка авторизации", false);
    }

    public async Task<Response<AppUser>> GetUserDetail(ClaimsPrincipal claimsPrincipal)
    {
        var result = await _userManager.GetUserAsync(claimsPrincipal);
        if (result is not null)
        {
            var roles = await _userManager.GetRolesAsync(result);
            result.Roles = new List<Microsoft.AspNetCore.Identity.IdentityRole>();
            foreach (var role in roles)
            {
                result.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole(role));
            }
            
            return new Response<AppUser>(result, true);
        }

        await _signInManager.SignOutAsync();
        return new Response<AppUser>("Ошибка в куках, зарегистрируйтесть заного", false);
    }
    
    
    public async Task<Response<AppUser>> ExitFromAccount()
    {
        await _signInManager.SignOutAsync();
        return new Response<AppUser>(true);
    }

    public async Task<Response<AppUser>> Edit(UserEditDto userEditDto, ClaimsPrincipal claimsPrincipal)
    {
        var userDb = await _userManager.GetUserAsync(claimsPrincipal);
        var email = await _userManager.ChangeEmailAsync(userDb, userEditDto.Email, "token");
        var userName = await _userManager.SetUserNameAsync(userDb, userEditDto.UserName);
        var password = await _userManager.ChangePasswordAsync(
            userDb, userEditDto.PasswordOld, userEditDto.PasswordNew);
        

        if (email.Succeeded && userName.Succeeded && password.Succeeded)
        {
            return new Response<AppUser>(true);
        }
        
        var response = new Response<AppUser>(false);
        foreach (var error in email.Errors)
        {
            response.Errors.Add(error.Description);
        }
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

    public async Task<Response<AppUser>> ConfirmEmail(string toEmail , string callbackUrl)
    {
        await _emailService.SendEmailAsync(toEmail, "Подтвердите почту в Mag",
            $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");
        return new Response<AppUser>(true);
    }

    public async Task<Response<string>> CheckConfirm(string userEmail, string code)
    {
        if (userEmail == null || code == null)
        {
            return new Response<string>("Ошибка подтверждения", false);
        }

        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
        {
            return new Response<string>("Ошибка подтверждения: не найден такой Email", false);
        }
        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            return new Response<string>(true);
        }
        else
        {
            var response = new Response<string>(false);
            response.Errors.Add( result.Errors.First().Description);
            return response;
        }
    }


    public async Task<Response<UserEditDto>> GetDataForEdit(ClaimsPrincipal claimsPrincipal)
    {
        var result = await _userManager.GetUserAsync(claimsPrincipal);
        
        if (result is not null)
        {
            var initData = new UserEditDto()
            {
                Email = result.Email,
                PasswordOld = result.PasswordHash,
                PasswordNew = result.PasswordHash,
                UserName = result.UserName
            };
            return new Response<UserEditDto>(initData);
        }
        return new Response<UserEditDto>("Ошибка, не найден пользователь");
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
}