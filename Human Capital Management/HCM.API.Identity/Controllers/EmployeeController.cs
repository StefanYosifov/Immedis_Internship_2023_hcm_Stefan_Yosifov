namespace HCM.API.Services.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Employees;
    using Models.ViewModels.Employees.Enum;

    using Services;
    using Services.Countries;
    using Services.Department;
    using Services.Employee;
    using Services.Gender;
    using System;

    [Route("/employees")]
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService service;
        private readonly IDepartmentService departmentService;
        private readonly IGenderService genderService;
        private readonly ICountryService countryService;
        public EmployeeController(
            IEmployeeService service,
            IDepartmentService departmentService,
            IGenderService genderService,
            ICountryService countryService)
        {
            this.service = service;
            this.departmentService = departmentService;
            this.genderService = genderService;
            this.countryService = countryService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployees([FromRoute] int id, [FromQuery] EmployeeQueryTableFilters query)
        {
            var employees = await service.GetEmployeeTable(id, query);

            return Ok(employees);
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

        [HttpGet("options")]
        public async Task<IActionResult> GetTableOptions()
        {
            var options = new EmployeeTableFilterOptions()
            {
                Genders = await genderService.GetGenders(),
                Counties = await countryService.GetCountries(),
                Departments = await departmentService.GetDepartments(),
                Sort = (string[])Enum.GetNames(typeof(EmployeeTableSortEnum))
            };

            return Ok(options);
        }

        [HttpGet("getCreate")]
        public async Task<IActionResult> GetEmployeeCreationOptions()
        {
            var employeeCreationModel = new EmployeeCreateRequestModel()
            {
                Options = new EmployeeCreateDropDownOptions()
                {
                    Departments = await departmentService.GetDepartments(),
                    Countries = await countryService.GetCountries(),
                    Genders = await genderService.GetGenders(),
                }
            };

            return Ok(employeeCreationModel);
        }

        [HttpPost("postCreate")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateResponseModel requestModel)
        {
            try
            {
                var result = await service.CreateEmployee(requestModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("postCreateFile")]
        public async Task<IActionResult> CreateEmployeesFromFile([FromForm] IFormFile file)
        {
            try
            {
                var result = await service.CreateFileFromEmployees(file);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
