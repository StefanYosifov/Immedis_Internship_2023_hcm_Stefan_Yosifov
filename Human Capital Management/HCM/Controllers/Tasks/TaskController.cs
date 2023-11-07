namespace HCM.Controllers.Tasks
{
    using Common.Requests;

    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Tasks;

    using RestSharp;

    using Method = RestSharp.Method;

    public class TaskController : BaseController
    {

        public async Task<IActionResult> TasksPartial(SearchTaskModel model)
        {
            var request = new RestRequestBuilder("/api/tasks/employee", GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddQueryParameter(model)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<EmployeeTasksModel>(request);

            if (response.IsSuccessful)
            {
                return PartialView(response.Data);
            }

            return View(model);
        }

        public async Task<IActionResult> My(int? id)
        {
            return View();
        }

        [HttpGet("tasks/pagination")]
        public async Task<IActionResult> TasksPaginationPartial(SearchEmployeeTasksPagination model)
        {
            var request = new RestRequestBuilder("/api/tasks/pagination", GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddQueryParameter(model)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<EmployeeTasksPagination>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Data);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskModel model)
        {
            var request=new RestRequestBuilder("/api/tasks/create",GetAuthenticationClaim())
                .SetMethod(Method.Post)
                .AddBody(model)
                .AddAuthentication()
                .Build();

            var response = await client.ExecutePostAsync<string>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Data);
        }

    }
}
