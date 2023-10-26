namespace HCM.Controllers.Identity
{
    using System.Net;

    using Common.Constants;
    using Data.Models;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.ViewModels.Identity;
    using RestSharp;
    using System.Security.Claims;
    using System.Net.Http.Headers;

    [AllowAnonymous]
    public class IdentityController : BaseController
    {
        private readonly RestClient client;

        public IdentityController()
        {
            this.client = new RestClient(ApplicationAPIConstants.API_BASE_URL);
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

        [HttpPost("/identity/SignIn")]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = new RestRequest("authorize/SignIn", Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(model);

            var response = await client.ExecuteAsync<Response>(request);
            var responseData = response.Data;


            if (response.IsSuccessStatusCode)
            {
                HttpContext.Request.Headers.Authorization = responseData.JwtToken;
                await AuthenticateUserAndSetupClaims(responseData.Employee);
                return Ok(responseData.JwtToken);
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SignIn");
        }



        private async Task AuthenticateUserAndSetupClaims(Employee user)
        {
            // Would be better to move this logic to the service
            var userRoles = user.EmployeeRoles
                .Where(er => er.EmployeeId == user.Id)
                .Select(er => er.Role.Name);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name,$"{user.FirstName} {user.LastName}"),
            };

            foreach (var userRole in userRoles)
            {
                claims.Append(new Claim(ClaimTypes.Role, userRole));
            }


            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties()
                {
                    IsPersistent = true
                });
        }
    }
}
