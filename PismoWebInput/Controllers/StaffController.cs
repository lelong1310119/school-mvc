using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PismoWebInput.Core.Infrastructure.Services;
using PismoWebInput.Core.Persistence.Contexts;

namespace PismoWebInput.Controllers
{
    [Authorize(Roles = "Staff")]
    public class StaffController : Controller
    {
        private readonly IStudentService studentService;
        private readonly IStaffService staffService;
        private readonly EfContext efContext;
        public StaffController(IStudentService studentService, EfContext efContext, IStaffService staffService)
        {
            this.studentService = studentService;
            this.efContext = efContext;
            this.staffService = staffService;   
        }

            // GET: StaffController
        public async Task<ActionResult> Index()
        {
            //var result = await staffService.GetAll(1);
            var result = await studentService.GetTotal("20230001");
            ViewData["Total"] = result;
            return View();
        }

        // GET: StaffController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StaffController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StaffController/Create
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

        // GET: StaffController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StaffController/Edit/5
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

        // GET: StaffController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StaffController/Delete/5
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
