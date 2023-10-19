﻿namespace HCM.API.Identity.Identity
{
    using Data.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Identity;

    using Services;

    [ApiController]
    [AllowAnonymous]
    [Route("/identity")]
    public class IdentityController : Controller
    {
        private readonly IIdentityService service;

        public IdentityController(IIdentityService service)
        {
            this.service = service;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            var result=await service.SignIn(model);
            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
