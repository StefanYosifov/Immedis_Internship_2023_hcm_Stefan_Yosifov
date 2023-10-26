﻿namespace HCM.Controllers.Home
{
    using HCM.Controllers;
    using HCM.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    using Microsoft.AspNetCore.Authorization;

    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Identity");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}