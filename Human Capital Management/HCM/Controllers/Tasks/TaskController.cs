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

        [HttpGet("tasks/options")]
        public async Task<IActionResult> CreateTasksPartial()
        {
            var request=new RestRequestBuilder("/api/tasks/options",GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<TaskCreationOptions>(request);

            if (response.IsSuccessful)
            {
                //return PartialView(response.Data);
            }

            return BadRequest();
        }

        public IActionResult My(int? id)
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

        [HttpPost("tasks/create")]
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

        [HttpPut("tasks/complete")]
        public async Task<IActionResult> CompleteTask([FromBody] int id)
        {
            var request=new RestRequestBuilder("/api/tasks/complete",GetAuthenticationClaim())
                .SetMethod(Method.Put)
                .AddBody(id)
                .AddAuthentication()
                .Build();

            var response=await client.ExecutePutAsync<string>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Data);
        }

        [HttpGet("tasks/issuedByMe/{page}")]
        public async Task<IActionResult> TasksIssuedByMe(int page)
        {
            var request = new RestRequestBuilder($"/api/tasks/issuedByMe/{page}", GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            var response= await client.ExecuteGetAsync<EmployeeTasksPagination>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Data);
        }
    }
}
