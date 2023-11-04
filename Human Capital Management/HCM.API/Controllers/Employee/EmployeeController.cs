namespace HCM.API.Controllers.Employee
{
    using Core.Services.Countries;
    using Core.Services.Department;
    using Core.Services.Employee;
    using Core.Services.Gender;

    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Employees;
    using Models.ViewModels.Employees.Enum;

    [Route("api/employees")]
    public class EmployeeController : ApiController
    {
        private readonly ICountryService countryService;
        private readonly IDepartmentService departmentService;
        private readonly IGenderService genderService;
        private readonly IEmployeeService service;

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
            var options = new EmployeeTableFilterOptions
            {
                Genders = await genderService.GetGenders(),
                Counties = await countryService.GetCountries(),
                Departments = await departmentService.GetDepartments(),
                Sort = Enum.GetNames(typeof(EmployeeTableSortEnum))
            };

            return Ok(options);
        }

        [HttpGet("getCreate")]
        public async Task<IActionResult> GetEmployeeCreationOptions()
        {
            var employeeCreationModel = new EmployeeCreateRequestModel
            {
                Options = new EmployeeCreateDropDownOptions
                {
                    Departments = await departmentService.GetDepartments(),
                    Countries = await countryService.GetCountries(),
                    Genders = await genderService.GetGenders()
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
        public async Task<IActionResult> CreateEmployeesFromFile(EmployeeFileInternalDTO file)
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

        [HttpGet("edit/{employeeId}")]
        public async Task<IActionResult> GetEmployeeToEdit(string employeeId)
        {
            try
            {
                var result = await service.GetEmployeeToEdit(employeeId);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("edit/{employeeId}")]
        public async Task<IActionResult> EditEmployee(string employeeId, EmployeeSendEditModel model)
        {
            try
            {
                var result = await service.EditEmployee(employeeId, model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("search/name/{name}")]
        public async Task<IActionResult> GetEmployeeWithNoDepartmentByName(string? name)
        {
            try
            {
                var result = await service.GetEmployeesWithNoDepartmentByName(name);
                return Ok(result);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPut("edit/position/seniority")]
        public async Task<IActionResult> EditEmployeesPositionAndSeniority(EmployeeEditPositionAndSeniority model)
        {
            try
            {
                var result = await service.EditEmployeePositionAndSeniority(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}