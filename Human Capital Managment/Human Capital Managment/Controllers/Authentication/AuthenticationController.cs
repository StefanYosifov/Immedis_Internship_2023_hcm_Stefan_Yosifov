namespace Human_Capital_Managment.Controllers.Authentication
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    using Human_Capital_Management.Services.UserDetails;

    using Human_Capital_Managment.Data.Models;

    using ViewModels.AuthenticationViewModels;
    using IAuthenticationService = Human_Capital_Management.Services.Authentication.IAuthenticationService;


    [AllowAnonymous]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService authService;
        private readonly IUserDetailsService detailsService;

        public AuthenticationController(
            IAuthenticationService authService,
            IUserDetailsService detailsService)
        {
            this.authService = authService;
            this.detailsService = detailsService;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            var loginView = new LoginViewModel();
            return View(loginView);
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var user = await authService.Login(loginModel);

            if (user == null)
            {
                return View();
            }

            await AuthenticateUserAndSetupClaims(user);

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult SignUp()
        {
            var registerView = new RegisterViewModel();
            return View(registerView);
        }


        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }

            var user = await authService.FindEmployeeByEmail(registerModel.Email);

            if (user != null)
            {
                return View();
            }

            return RedirectToAction("SignUpDetails", "Authentication", registerModel);
        }

        [HttpGet]
        public async Task<IActionResult> SignUpDetails(RegisterViewModel registerModel)
        {
            var secondRegisterModel = new RegisterSecondRequestModel()
            {
                RegisterViewModel = registerModel,
                UserDetailsRequest = await detailsService.GetUserDetailsViewModelOptions()
            };
            return View(secondRegisterModel);
        }

        [HttpPost]
        public async Task<IActionResult> SignUpDetails(RegisterSecondResponseModel registerModel)
        {
            
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