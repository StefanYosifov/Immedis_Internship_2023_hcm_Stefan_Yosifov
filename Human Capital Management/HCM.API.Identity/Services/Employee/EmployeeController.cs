namespace HCM.API.Services.Services.Employee
{
    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Employees;

    using Services;

    [Route("/employees")]
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService service;

        public EmployeeController(IEmployeeService service)
        {
            this.service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployees([FromRoute] int id, [FromQuery] EmployeeQueryTableFilters query)
        {
            var employees = await service.GetEmployeeTable(id, query);
            if (employees.Count == 0)
            {
                return BadRequest();
            }

            return Ok(employees);
        }

        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartments()
        {
            var departments=await service.GetDepartments();
            return Ok(departments);
        }

        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await service.GetCountries();
            return Ok(countries);
        }

        [HttpGet("genders")]
        public async Task<IActionResult> GetGenders()
        {
            var countries = await service.GetGenders();
            return Ok(countries);
        }

    }
}
