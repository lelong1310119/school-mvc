using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PismoWebInput.Core.Infrastructure.Services;
using System.Security.Claims;

namespace PismoWebInput.Controllers
{
    [Authorize(Roles = "Lecturer")]
    public class LecturerManagerController : Controller
    {
        private readonly ILecturerService _lecturerService;
        public LecturerManagerController(ILecturerService lecturerService)
        {
            _lecturerService = lecturerService;
        }

        // GET: LecturerManager
        public async Task<ActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _lecturerService.GetSubjectClass(userId);
            return View(result);
        }

        // GET: LecturerManager/Details/5
        public async Task<ActionResult> Detail(int id)
        {
            var result = await _lecturerService.GetRegisCode(id);
            return View(result);
        }

        // GET: LecturerManager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LecturerManager/Create
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

        // GET: LecturerManager/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LecturerManager/Edit/5
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

        // GET: LecturerManager/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LecturerManager/Delete/5
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
