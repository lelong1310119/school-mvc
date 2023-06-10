﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using PismoWebInput.Core.Enums;
using PismoWebInput.Core.Infrastructure.Extensions;
using PismoWebInput.Core.Infrastructure.Models;

namespace PismoWebInput.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole(AppRoleEnum.Admin.ToString()))
            {
                return View();
            } else if (User.IsInRole(AppRoleEnum.Staff.ToString())) {
                return RedirectToAction("Index", "Staff");
            }
            else if (User.IsInRole(AppRoleEnum.Lecturer.ToString()))
            {
                return RedirectToAction("Index", "LecturerManager");
            } 
            return RedirectToAction("Index", "RegisCourse");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var ctx = HttpContext;
            var feature = ctx.Features.Get<IExceptionHandlerPathFeature>();
            ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var exception = feature?.Error;

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = $"{exception?.Message} {exception?.StackTrace}" });
        }
    }
}