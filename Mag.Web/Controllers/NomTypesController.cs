using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mag.DAL;
using Mag.DAL.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Mag.Web.Controllers
{
    [Authorize(Policy = "Root")]
    public class NomTypesController : Controller
    {
        private readonly AppDbContext _context;

        public NomTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: NomTypes
        public async Task<IActionResult> Index()
        {
              return _context.NomTypes != null ? 
                          View(await _context.NomTypes.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.NomTypes'  is null.");
        }

        // GET: NomTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NomTypes == null)
            {
                return NotFound();
            }

            var nomType = await _context.NomTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nomType == null)
            {
                return NotFound();
            }

            return View(nomType);
        }

        // GET: NomTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NomTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] NomType nomType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nomType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nomType);
        }

        // GET: NomTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NomTypes == null)
            {
                return NotFound();
            }

            var nomType = await _context.NomTypes.FindAsync(id);
            if (nomType == null)
            {
                return NotFound();
            }
            return View(nomType);
        }

        // POST: NomTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] NomType nomType)
        {
            if (id != nomType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nomType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NomTypeExists(nomType.Id))
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
            return View(nomType);
        }

        // GET: NomTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NomTypes == null)
            {
                return NotFound();
            }

            var nomType = await _context.NomTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nomType == null)
            {
                return NotFound();
            }

            return View(nomType);
        }

        // POST: NomTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NomTypes == null)
            {
                return Problem("Entity set 'AppDbContext.NomTypes'  is null.");
            }
            var nomType = await _context.NomTypes.FindAsync(id);
            if (nomType != null)
            {
                _context.NomTypes.Remove(nomType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NomTypeExists(int id)
        {
          return (_context.NomTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
