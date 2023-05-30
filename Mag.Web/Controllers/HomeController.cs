using Mag.Common.ViewModels;
using Mag.DAL;
using Mag.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Mag.Web.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ActionResult> Index()
    {
        ViewBag.Types = await _context.NomTypes.ToListAsync();

        return _context.Noms != null ?
            View(await _context.Supply
            .Include(t => t.Nom)
            .Include(t => t.Stock)
            .ToListAsync()) :
            Problem("Entity set 'AppDbContext.Stocks'  is null.");
    }

    public async Task<ActionResult> Filter(int typeId)
    {
        ViewBag.Types = await _context.NomTypes.ToListAsync();

        return _context.Noms != null ?
            View("Index" ,await _context.Supply
            .Include(t => t.Nom)
            .Include(t => t.Stock)
            .Where(t => t.Nom.NType.Id == typeId)
            .ToListAsync()) :
            Problem("Entity set 'AppDbContext.Stocks'  is null.");
    }

    [HttpGet]
    public async Task<ActionResult> CreateOrder(long supplyId)
    {
        ViewBag.Types = await _context.NomTypes.ToListAsync();

        var Supply = _context.Supply
            .Include(t => t.Nom)
            .Include(t => t.Stock)
            .Single(t => t.Id == supplyId);

        var vm = new CreateOrderVM()
        {
            SupplyId = Supply.Id,
            NType = Supply.Nom.NType.Title,
            PhotoName = Supply.Nom.PhotoName,
            Price = Supply.Nom.Price,
            Title = Supply.Nom.Title,
            ShelfLife = Supply.Nom.ShelfLife,
        };
        
        return View(vm);
    }


    [HttpPost]
    [Authorize]
    public async Task<ActionResult> AddOrder(int capacity, long supplyId, string adres)
    {
        ViewBag.Types = await _context.NomTypes.ToListAsync();
        var suply = await _context.Supply.SingleAsync(t => t.Id == supplyId);
        if(suply.Capacity < capacity)
        {
            return View("Error");
        }

        suply.Capacity -= capacity;
        if(suply.Capacity == 0)
        {
            _context.Supply.Remove(suply);
        }
        else
        {
            _context.Supply.Update(suply);
        }

        _context.Orders.Add(new Order
        {
            Adres = adres,
            AppUser = _context.AppUser.Single(u => u.Email == User.FindFirst(ClaimTypes.Email).Value),
            Date = DateTime.Now,
            Quantity = capacity,
            Supply = _context.Supply.Single(t => t.Id == supplyId)
        });

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}


