using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PismoWebInput.Core.Infrastructure.Models.LecturerModel;
using PismoWebInput.Core.Infrastructure.Models.StudentModel;
using PismoWebInput.Core.Infrastructure.Services;

namespace PismoWebInput.Controllers
{
    [Authorize()]
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;
        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        // GET: LecturerController
        public async Task<ActionResult> Index()
        {
            ViewData["Items"] = await _studentService.GetAll();
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm] StudentModel model)
        {
            try
            {
                await _studentService.Create(model);
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
