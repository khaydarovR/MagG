using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mag.DAL;
using Mag.DAL.Entities;
using Microsoft.IdentityModel.Tokens;
using Mag.Common.ViewModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Mag.Web.Controllers
{
    [Authorize(Policy = "Root")]
    public class SuppliesController : Controller
    {
        private readonly AppDbContext _context;

        public SuppliesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Supplies
        public async Task<IActionResult> Index()
        {
              return _context.Supply != null ? 
                          View(await _context.Supply
                          .Include(t => t.User)
                          .Include(t => t.Nom)
                          .Include(t => t.Stock)
                          .ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Supply'  is null.");
        }

        // GET: Supplies/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Supply == null)
            {
                return NotFound();
            }

            var supply = await _context.Supply
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supply == null)
            {
                return NotFound();
            }

            return View(supply);
        }

        // GET: Supplies/Create
        public async Task<ActionResult>Create()
        {
            ViewBag.Stocks = await _context.Stocks.ToListAsync();
            ViewBag.Noms = await _context.Noms.ToListAsync();
            ViewBag.Users = await _context.AppUser.ToListAsync();

            return View();
        }

        // POST: Supplies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Supply supply)
        {
            if (supply.DateTime != DateTime.MinValue && supply.Capacity != 0)
            {
                supply.User = await _context.Users.SingleAsync(m => m.Id == supply.User.Id);
                supply.Stock = await _context.Stocks.SingleAsync(m => m.Id == supply.Stock.Id);
                supply.Nom = await _context.Noms.SingleAsync(m => m.Id == supply.Nom.Id);

                _context.Add(supply);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Stocks = await _context.Stocks.ToListAsync();
            ViewBag.Noms = await _context.Noms.ToListAsync();
            ViewBag.Users = await _context.AppUser.ToListAsync();
            return View(supply);
        }

        // GET: Supplies/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Supply == null)
            {
                return NotFound();
            }

            var supply = await _context.Supply.FindAsync(id);
            if (supply == null)
            {
                return NotFound();
            }
            return View(supply);
        }

        // POST: Supplies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,DateTime,Capacity")] Supply supply)
        {
            if (id != supply.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supply);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplyExists(supply.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(supply);
        }

        // GET: Supplies/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Supply == null)
            {
                return NotFound();
            }

            var supply = await _context.Supply
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supply == null)
            {
                return NotFound();
            }

            return View(supply);
        }

        // POST: Supplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Supply == null)
            {
                return Problem("Entity set 'AppDbContext.Supply'  is null.");
            }
            var supply = await _context.Supply.FindAsync(id);
            if (supply != null)
            {
                _context.Supply.Remove(supply);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplyExists(long id)
        {
          return (_context.Supply?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
