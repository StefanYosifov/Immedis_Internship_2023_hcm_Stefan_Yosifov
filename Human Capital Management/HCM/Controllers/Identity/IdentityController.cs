namespace HCM.Controllers.Identity
{
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Text;
    using Common.Constants;

    using Data.Models;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Identity;

    using Newtonsoft.Json;

    using RestSharp;

    [AllowAnonymous]
    public class IdentityController : BaseController
    {
        private RestClient client;

        public IdentityController()
        {
            this.client = new RestClient(ApplicationURLConstants.API_BASE_URL);
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var request = new RestRequest("identity/SignIn", Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(model);

            var response = client.Execute<Employee>(request);
            var employee = response.Data;


            if (response.IsSuccessStatusCode)
            {
                await AuthenticateUserAndSetupClaims(employee!);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }



        private async Task AuthenticateUserAndSetupClaims(Employee user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name,$"{user.FirstName} {user.LastName}")
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
