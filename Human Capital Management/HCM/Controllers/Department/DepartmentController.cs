namespace HCM.Controllers.Department
{
    using Common.Requests;

    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Departments;
    using Models.ViewModels.Positions;
    using Models.ViewModels.Seniorities;

    using RestSharp;


    [Route("/departments")]
    public class DepartmentController : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet("positions/{departmentId}")]
        public async Task<IActionResult> GetPositionsByDepartmentId([FromRoute] int departmentId)
        {
            var request = new RestRequestBuilder($"/api/departments/positions/{departmentId}",
                    GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<ICollection<PositionViewModel>>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest();
        }

        [HttpGet("seniorities/{positionId}")]
        public async Task<IActionResult> GetSenioritiesByPositionId(int positionId)
        {
            var request = new RestRequestBuilder($"/api/departments/seniorities/{positionId}",
                    GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<ICollection<SeniorityViewModel>>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            TempData["Error"] = "This is some error";
            return BadRequest();
        }

        [HttpGet("all/main")]
        public async Task<IActionResult> GetAllDepartments([FromRoute] DepartmentSendQueryFilters query)
        {
            var request = new RestRequestBuilder("/api/departments/all/main", 
                    GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddParameter(query.Search, query.CountryId.ToString(), query.Sort.ToString())
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<ICollection<DepartmentGetAllModel>>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Content);
            }

            return BadRequest();
        }

        [HttpGet("options")]
        public async Task<IActionResult> GetDepartmentsFilterOptions()
        {
            var request = new RestRequestBuilder("/api/departments/options", 
                   GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteAsync<DepartmentGetAllQueryFilters>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Content);
            }

            return BadRequest();
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var request = new RestRequestBuilder($"/api/departments/details/{id}", GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            
            var response = await client.ExecuteGetAsync<DepartmentDetailsViewModel>(request);

            if (response.IsSuccessful)
            {
                return View(response.Data);
            }

            return BadRequest();
        }

        [HttpPost("positions/add")]
        public async Task<IActionResult> AddPositionToDepartment([FromBody] DepartmentAddPosition model)
        {
            var request = new RestRequestBuilder("/api/departments/positions/add",
                    GetAuthenticationClaim())
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

        [HttpDelete("positions/remove")]
        public async Task<IActionResult> RemovePositionFromDepartment([FromBody] DepartmentRemovePosition model)
        {
            var request = new RestRequestBuilder("/api/departments/positions/remove",
                GetAuthenticationClaim())
                .SetMethod(Method.Delete)
                .AddBody(model)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteAsync<string>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Data);
        }

    }
}
