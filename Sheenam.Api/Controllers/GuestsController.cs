using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sheenam.Api.Controllers
{
    public class GuestsController : Controller
    {
        // GET: GuestsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: GuestsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GuestsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GuestsController/Create
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

        // GET: GuestsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GuestsController/Edit/5
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

        // GET: GuestsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GuestsController/Delete/5
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
