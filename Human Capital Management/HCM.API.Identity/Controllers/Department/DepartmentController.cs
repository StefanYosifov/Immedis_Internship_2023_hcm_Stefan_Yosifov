namespace HCM.API.Controllers.Department
{
    using Common.Exceptions_Messages.Departments;

    using Core.Services.Department;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Departments;

    [Route("/api/departments")]
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
        public async Task<IActionResult> GetAllDepartments([FromQuery] DepartmentSendQueryFilters query)
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

        [Authorize]
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetDepartmentDetails(int id)
        {
            try
            {
                var department = await service.GetDepartmentDetailsById(id);
                return Ok(department);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost("positions/add")]
        public async Task<IActionResult> AddPositionToDepartment(DepartmentAddPosition model)
        {
            try
            {
                var result = await service.AddPositionToDepartmentById(model);
                return Created("asd", result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpDelete("positions/remove")]
        public async Task<IActionResult> RemovePositionFromDepartment(DepartmentRemovePosition model)
        {
            try
            {
                var result = await service.RemovePositionFromDepartmentById(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("employee/add")]
        public async Task<IActionResult> AddEmployeeToDepartment(DepartmentAddEmployee model)
        {
            try
            {
                var result = await service.AddEmployeeToDepartmentById(model);
                return Created("", result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("employee/remove")]
        public async Task<IActionResult> RemoveEmployeeFromDepartment(DepartmentRemoveEmployee model)
        {
            try
            {
                var result = await service.RemoveEmployeeFromDepartmentById(model);
                return Ok(result);
            }
            catch (DepartmentServiceExceptions e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Bad request");
            }
        }
    }
}