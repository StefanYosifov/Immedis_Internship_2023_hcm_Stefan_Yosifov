namespace Human_Capital_Managment.Controllers.Authentication
{
    using Data.Models2;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;

    using ViewModels.AuthenticationViewModels;
    using IAuthenticationService = Human_Capital_Management.Services.Authentication.IAuthenticationService;


    [AllowAnonymous]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService service;

        public AuthenticationController(IAuthenticationService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> SignIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var user = await service.Login(loginModel);

            if (user == null)
            {
                return View();
            }

            await AuthenticateUserAndSetupClaims(user);

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }

            var user = await service.Register(registerModel);

            if (user == null)
            {
                return View();
            }

            await AuthenticateUserAndSetupClaims(user);

            return RedirectToAction("Index", "Home");
        }

        private async Task AuthenticateUserAndSetupClaims(Employee user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.NameIdentifier, user.Id)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }
    }
}