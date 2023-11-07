namespace HCM.API.Controllers.Admin
{
    using Core.Services.Admin;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Admin;

    [Authorize(Roles = "Admin")]
    [Route("/api/admin")]
    public class AdminController : ApiController
    {
        private readonly IAdminService service;

        public AdminController(IAdminService service)
        {
            this.service = service;
        }

        [HttpGet("audit")]
        public async Task<IActionResult> GetAuditLogs([FromQuery]int page)
        {
            var result=await service.GetAuditLogs(page);
            return Ok(result);
        }

        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees([FromQuery]AdminSearchEmployee model)
        {
            var result=await service.GetEmployees(model);
            return Ok(result);
        }

    }
}
