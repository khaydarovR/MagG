using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mag.DAL;
using Mag.DAL.Entities;
using Mag.Common;
using Mag.Common.ViewModels;

namespace Mag.Web.Controllers
{
    public class NomsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public NomsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Noms
        public async Task<IActionResult> Index()
        {
              return _context.Noms != null ? 
                          View(await _context.Noms.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Noms'  is null.");
        }

        // GET: Noms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Noms == null)
            {
                return NotFound();
            }

            var nom = await _context.Noms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nom == null)
            {
                return NotFound();
            }

            return View(nom);
        }

        // GET: Noms/Create
        public IActionResult Create()
        {
            ViewBag.Types = _context.NomTypes.ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateNomVM model)
        {
            ModelState.MaxAllowedErrors = 1;
            if (ModelState.IsValid)
            {
                Nom newNom = await MapToDb(model);
                _context.Add(newNom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Types = _context.NomTypes.ToList();

            return View(model);
        }

        private async Task<Nom> MapToDb(CreateNomVM model)
        {
            var photoName = await SaveImage(model.Photo);
            var type = await _context.NomTypes.SingleAsync(m => m.Id == int.Parse(model.NType));

            var newNom = new Nom()
            {
                NType = type,
                PhotoName = photoName,
                Price = model.Price,
                ShelfLife = model.ShelfLife,
                Title = model.Title
            };
            return newNom;
        }

        // GET: Noms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Noms == null)
            {
                return NotFound();
            }

            var nom = await _context.Noms.FindAsync(id);
            if (nom == null)
            {
                return NotFound();
            }
            return View(nom);
        }

        // POST: Noms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,PhotoName,ShelfLife,Price")] Nom nom)
        {
            if (id != nom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NomExists(nom.Id))
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
            return View(nom);
        }

        // GET: Noms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Noms == null)
            {
                return NotFound();
            }

            var nom = await _context.Noms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nom == null)
            {
                return NotFound();
            }

            return View(nom);
        }

        // POST: Noms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Noms == null)
            {
                return Problem("Entity set 'AppDbContext.Noms'  is null.");
            }
            var nom = await _context.Noms.FindAsync(id);
            if (nom != null)
            {
                DeleteOldImage(nom.PhotoName);
                _context.Noms.Remove(nom);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NomExists(int id)
        {
          return (_context.Noms?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<string> SaveImage(IFormFile model)
        {
            if(model is null)
            {
                return Literals.DefaultProdImgName;
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(_env.WebRootPath + Literals.PathForProdImg);
            if (directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            string fileName = Path.GetFileNameWithoutExtension(model.Length.ToString());
            string extension = Path.GetExtension(model.FileName).ToLower();

            string fulFilename = DateTime.Now.Hour.ToString()
                + DateTime.Now.DayOfYear.ToString()
                + DateTime.Now.Minute.ToString()
                + model.GetHashCode().ToString()
                + fileName + extension;
            string fullPath = Path.Combine(_env.WebRootPath + Literals.PathForProdImg + fulFilename);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await model.CopyToAsync(fileStream);
            }

            return fulFilename;
        }

        private void DeleteOldImage(string imageTitle)
        {
            if (imageTitle != Literals.DefaultProdImgName)
            {
                FileInfo file = new FileInfo(Path.Combine(_env.WebRootPath + Literals.PathForProdImg + imageTitle));
                file.Delete();
            }
        }

        private void ResetImage(int id)
        {
            var prod = _context.Noms.First(db => db.Id == id);
            if (prod.PhotoName != Literals.DefaultProdImgName)
            {
                FileInfo file = new FileInfo(Path.Combine(_env.WebRootPath + Literals.PathForProdImg + prod));
                file.Delete();
                prod.PhotoName = Literals.DefaultProdImgName;
                _context.Update(prod);
                _context.SaveChanges();
            }
        }
    }
}
