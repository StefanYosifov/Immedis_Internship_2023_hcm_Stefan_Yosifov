namespace HCM.Controllers.Admin
{
    using Common.Requests;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Admin;

    using RestSharp;

    using Method = RestSharp.Method;

    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {

        public IActionResult Index()
        {

            return View();
        }

        [HttpGet("admin/audit")]
        public async Task<IActionResult> GetAuditLogs([FromQuery]int page)
        {
            var request=new RestRequestBuilder("/api/admin/audit",GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            request.AddParameter("page", page);

            var response = await client.ExecuteGetAsync<AuditLogsPagination>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Data);  
        }

        [HttpGet("admin/employees")]
        public async Task<IActionResult> GetEmployees([FromQuery]AdminSearchEmployee model)
        {
            var request=new RestRequestBuilder("/api/admin/employees",GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddQueryParameter(model)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<AdminEmployeePagination>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Data);
        }
        [HttpGet("admin/departments")]
        public async Task<IActionResult> GetDepartments()
        {
            var request = new RestRequestBuilder("/api/admin/departments", GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<AdminDepartmentsCollection>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Data);
        }

        [HttpPost("admin/departments/create")]
        public async Task<IActionResult> CreateDepartment([FromBody]AdminCreateDepartment model)
        {
            var request = new RestRequestBuilder("/api/admin/departments/create", GetAuthenticationClaim())
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
