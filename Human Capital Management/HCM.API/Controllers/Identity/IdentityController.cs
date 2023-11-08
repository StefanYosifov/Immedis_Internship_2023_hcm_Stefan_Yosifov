namespace HCM.API.Controllers.Identity
{
    using Core.Services.Identity;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Identity;

    [AllowAnonymous]
    [Route("/api/authorize")]
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

            if (result == null || result.isValid==false)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPut("ChangePass")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            try
            {
                var result = await service.ChangePassword(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}