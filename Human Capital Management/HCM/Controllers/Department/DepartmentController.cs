namespace HCM.Controllers.Department
{
    using System.Security.Claims;

    using Common.Requests;

    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Positions;
    using Models.ViewModels.Seniorities;

    using RestSharp;

    [ApiController]
    [Route("MVC/departments")]
    public class DepartmentController : BaseController
    {

        [HttpGet("positions/{departmentId}")]
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

        [HttpGet("seniorities/{positionId}")]
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

    }
}
