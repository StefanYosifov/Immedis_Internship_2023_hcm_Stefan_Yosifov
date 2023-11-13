namespace HCM.API.Controllers.Tasks
{
    using Common.Exceptions_Messages.Tasks;

    using Core.Services.Task;

    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Tasks;

    [Route("/api/tasks")]
    public class TaskController : ApiController
    {
        private readonly ITaskService service;

        public TaskController(ITaskService service)
        {
            this.service = service;
        }

        [HttpGet("employee")]
        public async Task<IActionResult> GetEmployeeTask([FromQuery]SearchTaskModel model)
        {
            var result = await service.GetEmployeeTasks(model);
            return Ok(result);
        }

        [HttpGet("options")]
        public async Task<IActionResult> GetTaskOptions()
        {
            var result = await service.GetTaskOptions();
            return Ok(result);
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetEmployeePaginatedTasks([FromQuery]SearchEmployeeTasksPagination model)
        {
            var result = await service.GetEmployeeTasksInPaginationFormat(model);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTask([FromBody]CreateTaskModel model)
        {
            try
            {
                var result = await service.CreateTask(model);
                return Ok(result);
            }
            catch (TaskServiceExceptions e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(TaskMessages.InvalidRequest);
            }
        }

        [HttpPut("complete")]
        public async Task<IActionResult> CompleteTask([FromBody] int id)
        {
            try
            {
                var result = await service.MarkAsCompleted(id);
                return Ok(result);
            }
            catch (TaskServiceExceptions e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ca)
            {
                return BadRequest(TaskMessages.InvalidRequest);
            }
        }

        [HttpGet("issuedByMe/{page}")]
        public async Task<IActionResult> GetTaskIssuedByMe(int page)
        {
            var result = await service.TasksIssuedByMe(page);
            return Ok(result);
        }
    }
}
