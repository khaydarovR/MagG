using Mag.Common;
using Mag.Common.Interfaces;
using Mag.Common.Models;
using Mag.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mag.Web.Controllers;

public class AccountController: Controller
{
    private readonly IAccountService<AppUser> _accountService;
    
    public AccountController(IAccountService<AppUser> userService)
    {
        _accountService = userService;
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
    
        var response = await _accountService.AddUser(model);
        if (response.IsSuccessful)
        {
            return RedirectToAction("Detail");
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
        
        var response = await _accountService.LoginUser(model);
        if (response.IsSuccessful)
        {
            return RedirectToAction("Detail");
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
        var response = await _accountService.GetUserDetail(HttpContext.User);
        
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
        var response = await _accountService.ExitFromAccount();
        
        if (response.IsSuccessful)
        {
            return RedirectToAction("Login");
        }

        return RedirectToAction("Detail");
    }


    [Authorize]
    public async Task<ActionResult> Edit()
    {
        var initData = await _accountService.GetUserDetail(HttpContext.User);
        if (!initData.IsSuccessful)
        {
            return View("Error");
        }
        var editDto = new UserEditDto()
            {UserName = initData.ResponseModel.UserName };
        return View(editDto);
    }
    
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Edit(UserEditDto userEditDto)
    {
        if (!ModelState.IsValid)
        {
            return View(userEditDto);
        }
        var response = await _accountService.Edit(userEditDto, HttpContext.User);
        
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
}