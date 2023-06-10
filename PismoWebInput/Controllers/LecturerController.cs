using Autofac.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PismoWebInput.Core.Infrastructure.Models.LecturerModel;
using PismoWebInput.Core.Infrastructure.Services;

namespace PismoWebInput.Controllers
{
    [Authorize()]
    public class LecturerController : Controller
    {
        private readonly ILecturerService _lecturerService;
        public LecturerController(ILecturerService lecturerService)
        {
            _lecturerService = lecturerService;
        }
        // GET: LecturerController
        public async Task<ActionResult> Index()
        {
            ViewData["Items"] = await _lecturerService.GetAll();
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm] LecturerModel model)
        {
            try
            {
                await _lecturerService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}
