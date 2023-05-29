using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mag.Web.Controllers
{
    public class NomController : Controller
    {
        // GET: NomController
        public ActionResult Index()
        {
            return View();
        }

        // GET: NomController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NomController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NomController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NomController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NomController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NomController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NomController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
