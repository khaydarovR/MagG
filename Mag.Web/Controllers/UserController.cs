using Mag.BL;
using Mag.Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;

namespace Mag.Web.Controllers;

[Authorize(Policy = "Root")]
public class UserController: Controller
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    public async Task<ActionResult> Index()
    {
        var response = await _userService.GetAllUsers();
        return View(response.ResponseModel);
    }
    
    public async Task<ActionResult> Edit(string guid)
    {
        var response = await _userService.GetUserForEdit(guid);
        SelectList roles =
            new SelectList(response.ResponseModel.Roles, response.ResponseModel.User.Role);
        ViewData["roles"] = roles;
        return View(response.ResponseModel);
    }
    
    [HttpPost]
    public async Task<ActionResult> Edit(UserEditVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
    
        var response = await _userService.SaveChange(model);
        if (response.IsSuccessful)
        {
            return RedirectToAction("Index");
        }

        foreach (var e in response.Errors)
        {
            ModelState.AddModelError("", e);
        }
        return View(model);
    }
    
}