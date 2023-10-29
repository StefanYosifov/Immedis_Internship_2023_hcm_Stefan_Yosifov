namespace HCM.Controllers.Department
{
    using System.Security.Claims;

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

        [HttpGet("MVC/positions/{departmentId}")]
        public async Task<IActionResult> GetPositionsByDepartmentId([FromRoute] int departmentId)
        {
            var request = new RestRequestBuilder($"/departments/positions/{departmentId}",
                    HttpContext.User.FindFirstValue(ClaimTypes.Authentication))
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

        [HttpGet("MVC/seniorities/{positionId}")]
        public async Task<IActionResult> GetSenioritiesByPositionId(int positionId)
        {
            var request = new RestRequestBuilder($"/departments/seniorities/{positionId}",
                    HttpContext.User.FindFirstValue(ClaimTypes.Authentication))
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

        [HttpGet("MVC/all/main")]
        public async Task<IActionResult> GetAllDepartments(DepartmentSendQueryFilters query)
        {
            var request = new RestRequestBuilder("/departments/all/main", HttpContext.User.FindFirstValue(ClaimTypes.Authentication))
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

        [HttpGet("MVC/options")]
        public async Task<IActionResult> GetDepartmentsFilterOptions()
        {
            var request=new RestRequestBuilder("/departments/options",HttpContext.User.FindFirstValue(ClaimTypes.Authentication))
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

    }
}
