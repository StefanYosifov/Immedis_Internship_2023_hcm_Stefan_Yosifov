namespace HCM.API.Controllers.Admin
{
    using Common.Exceptions_Messages.Admin;
    using Common.Exceptions_Messages.Departments;

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
        public async Task<IActionResult> GetAuditLogs([FromQuery] int page)
        {
            var result = await service.GetAuditLogs(page);
            return Ok(result);
        }

        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees([FromQuery] AdminSearchEmployee model)
        {
            var result = await service.GetEmployees(model);
            return Ok(result);
        }

        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartments()
        {
            var result = await service.GetDepartments();
            return Ok(result);
        }

        [HttpPost("departments/create")]
        public async Task<IActionResult> CreateDepartment([FromBody]AdminCreateDepartment model)
        {
            try
            {
                var result = await service.CreateDepartment(model);
                return Ok(result);
            }
            catch (DepartmentServiceExceptions e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return BadRequest(AdminMessages.InvalidRequest);
            }
        }

        [HttpPut("roles/change")]
        public async Task<IActionResult> ChangeRole([FromBody]AdminChangeRole model)
        {
            try
            {
                var result = await service.ChangeEmployeeRole(model);
                return Ok(result);
            }
            catch (AdminServiceExceptions e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
