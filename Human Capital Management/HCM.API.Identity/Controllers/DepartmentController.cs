namespace HCM.API.Controllers
{
    using Core.Services.Department;

    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Departments;

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

        [HttpGet("options")]
        public async Task<IActionResult> GetDepartmentsSortOptions()
        {
            var sortOptions = await service.GetAllQueryFilters();
            return Ok(sortOptions);
        }

        [HttpGet("all/main")]
        public async Task<IActionResult> GetAllDepartments([FromQuery]DepartmentSendQueryFilters query)
        {
            try
            {
                var departments = await service.GetAllDepartments(query);
                return Ok(departments);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
