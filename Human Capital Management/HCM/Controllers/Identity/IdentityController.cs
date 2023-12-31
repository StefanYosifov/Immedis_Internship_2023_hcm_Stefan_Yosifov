﻿namespace HCM.Controllers.Identity
{
    using System.Security.Claims;

    using Common.Requests;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Identity;

    using RestSharp;

    [AllowAnonymous]
    public class IdentityController : BaseController
    {
        [HttpGet]
        public IActionResult SignIn()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost("/identity/SignIn")]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            var request = new RestRequestBuilder("/api/authorize/SignIn", "")
                .SetMethod(Method.Post)
                .AddBody(model)
                .AddAuthentication()
                .Build();

            var response = await client.ExecutePostAsync<Response>(request);
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

        [HttpPut("/identity/ChangePass")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var request = new RestRequestBuilder("/api/authorize/ChangePass",
                    GetAuthenticationClaim())
                .SetMethod(Method.Put)
                .AddBody(model)
                .AddAuthentication()
                .Build();

            var response = await client.ExecutePutAsync<string>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Data);
        }

        private async Task AuthenticateUserAndSetupClaims(string token, EmployeeResponse user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Authentication, token),
                new(ClaimTypes.Hash, token)
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
                RedirectUri = Url.Action("Index", "Home")
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}