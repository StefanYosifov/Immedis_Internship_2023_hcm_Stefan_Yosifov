namespace HCM.API.Services.Controllers
{
    using Common.Constants;

    using Microsoft.AspNetCore.Mvc;

    using Services;
    using Services.Department;

    [Route("/departments")]
    public class DepartmentController : ApiController
    {
        private readonly IDepartmentService service;

        public DepartmentController(IDepartmentService service)
        {
            this.service = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await service.GetDepartments();

            return Ok(departments);
        }

        [HttpGet("positions/{departmentId}")]
        public async Task<IActionResult> GetPositionsByDepartmentId(int departmentId)
        {
            var positions = await service.GetPositionsByDepartmentId(departmentId);

            return Ok(positions);
        }


        [HttpGet("seniorities/{positionId}")]
        public async Task<IActionResult> GetSenioritiesByPositionId(int positionId)
        {
            var seniorities = await service.GetSenioritiesByPositionId(positionId);

            return Ok(seniorities);
        }
    }
}
