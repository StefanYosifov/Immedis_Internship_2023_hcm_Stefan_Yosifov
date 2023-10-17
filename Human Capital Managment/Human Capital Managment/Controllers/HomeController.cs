namespace Human_Capital_Managment.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;

    using Human_Capital_Management.Services.Home;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Models;

    public class HomeController : BaseController
    {
        private readonly IHomeService service;

        public HomeController(
            IHomeService service)
        {
            this.service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("SignIn", "Authentication");
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