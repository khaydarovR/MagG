using Mag.Common.Interfaces;
using Mag.Common.Models;
using Mag.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mag.Web.Controllers;

public class Account : Controller
{
    private readonly IUserService<AppUser> _userService;


    public Account(IUserService<AppUser> userService)
    {
        _userService = userService;
    }
    
    
    public IActionResult Register()
    {
        var model = new UserRegisterDto();
        return View("Register", model);
    }

    
    [HttpPost]
    public async Task<ActionResult> Register(UserRegisterDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
    
        var response = await _userService.AddUser(model);
        if (response.IsSuccessful)
        {
            var callbackUrl = Url.Action("ConfirmEmail", "Account",
                new { userEmail = model.Email, code = response.ResponseModel },
                protocol: HttpContext.Request.Scheme);
            
            await _userService.ConfirmEmail(model.Email ,callbackUrl);
            return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
        }

        foreach (var e in response.Errors)
        {
            ModelState.AddModelError("", e);
        }
        return View(model);
    }
    
    
    public ActionResult Login()
    {
        var model = new UserLoginDto();
        return View("Login", model);
    }
    
    
    [HttpPost]
    public async Task<ActionResult> Login(UserLoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var response = await _userService.LoginUser(model);
        if (response.IsSuccessful)
        {
            return RedirectToAction("Detail", response.ResponseModel);
        }
        else
        {
            foreach (var e in response.Errors)
            {
                ModelState.AddModelError("", e);
            }
            return View(model);
        }
    }

    
    [Authorize]
    public async Task<ActionResult> Detail()
    {
        var response = await _userService.GetUserDetail(HttpContext.User);
        
        if (response.IsSuccessful)
        {
            return View(response.ResponseModel);
        }

        foreach (var e in response.Errors)
        {
            ModelState.AddModelError("", e);
        }
        return View();
    }


    [Authorize]
    public async Task<ActionResult> Logout()
    {
        var response = await _userService.ExitFromAccount();
        
        if (response.IsSuccessful)
        {
            return RedirectToAction("Login");
        }

        return RedirectToAction("Detail");
    }


    [Authorize]
    public ActionResult Edit()
    {
        return View();
    }
    
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Edit(UserEditDto userEditDto)
    {
        if (!ModelState.IsValid)
        {
            return View(userEditDto);
        }
        var response = await _userService.Edit(userEditDto, HttpContext.User);
        
        if (response.IsSuccessful)
        {
            return RedirectToAction("Detail");
        }

        foreach (var e in response.Errors)
        {
            ModelState.AddModelError("", e);
        }
        return View();
    }
    

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userEmail, string code)
    {
        var response = await _userService.CheckConfirm(userEmail, code);

        if (response.IsSuccessful)
        {
            return RedirectToAction("Detail");
        }
        var errors = response.Errors;
        return View("Error", errors);
    }
}