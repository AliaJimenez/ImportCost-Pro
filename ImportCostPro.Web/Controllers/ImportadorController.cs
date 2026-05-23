using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImportCostPro.Web.Controllers
{
    public class ImportadorController : Controller
    {
        // GET: ImportadorController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ImportadorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ImportadorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImportadorController/Create
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

        // GET: ImportadorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ImportadorController/Edit/5
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

        // GET: ImportadorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ImportadorController/Delete/5
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
