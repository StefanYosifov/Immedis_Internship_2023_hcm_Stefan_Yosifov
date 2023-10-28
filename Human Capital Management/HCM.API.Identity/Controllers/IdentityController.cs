namespace HCM.API.Controllers
{
    using Core.Services.Identity;
    using Models.ViewModels.Identity;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    [Route("/authorize")]
    public class IdentityController : ApiController
    {
        private readonly IIdentityService service;

        public IdentityController(IIdentityService service)
        {
            this.service = service;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            var result = await service.SignIn(model);

            if (!result.isValid)
            {
                return BadRequest();
            }

            return Ok(result);

        }
    }
}
