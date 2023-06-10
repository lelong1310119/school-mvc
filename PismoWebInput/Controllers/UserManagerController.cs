using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PismoWebInput.Core.Infrastructure.Models.UserManager;
using PismoWebInput.Core.Infrastructure.Services;

namespace PismoWebInput.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagerController : Controller
    {
        private readonly IUserManagerService _service;

        public UserManagerController(
            IUserManagerService service
            )
        {
            _service = service;
        }

        public async Task<ActionResult> Index()
        {
            return View(await _service.GetAll());
        }

        public async Task<ActionResult> Create()
        {
            return View(new CreateUserManagerModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm] CreateUserManagerModel model)
        {
            try
            {
                await _service.CreateUser(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            return View(await _service.GetUser(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([FromForm] EditUserManagerModel model)
        {
            try
            {
                await _service.EditUser(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _service.DeleteUser(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index");
            }
        }
    }
}
