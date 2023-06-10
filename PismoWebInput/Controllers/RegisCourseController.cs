using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PismoWebInput.Core.Infrastructure.Services;
using System.Security.Claims;

namespace PismoWebInput.Controllers
{
    [Authorize(Roles = "Student")]
    public class RegisCourseController : Controller
    {
        private readonly IStudentService _studentService;
        public RegisCourseController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<ActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result1 = await _studentService.GetRegisteredSubjectClass(userId);
            var result2 = await _studentService.GetUnregisteredSubjectClass(userId);
            ViewData["Registered"] = result1;
            ViewData["Unregistered"] = result2;
            return View();
        }
    }
}
