using Microsoft.AspNetCore.Mvc;

namespace Mag.Web.Controllers;

public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}