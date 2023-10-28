﻿namespace HCM.Controllers.Identity
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.ViewModels.Identity;
    using RestSharp;
    using System.Security.Claims;

    [AllowAnonymous]
    public class IdentityController : BaseController
    {

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
                await AuthenticateUserAndSetupClaims(responseData.JwtToken, responseData.Employee);
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



        private async Task AuthenticateUserAndSetupClaims(string token, EmployeeResponse user)
        {

            
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name,user.Username),
                new(ClaimTypes.Authentication,token),
            };

            foreach (var userRole in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);


            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(10),
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow,
                RedirectUri = Url.Action("Index", "Home"),
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}
