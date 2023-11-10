// ReSharper disable InconsistentNaming

namespace HCM.Controllers.Employee
{
    using Common.Helpers;
    using Common.Requests;

    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Employees;

    using RestSharp;

    public class EmployeeController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> All(int id, [FromQuery] EmployeeQueryTableFilters query)
        {
            var request = new RestRequest($"/api/employees/{id}");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authentication", $"Bearer {Request.Headers["Authentication"]}");

            var DepartmentId = query.DepartmentId.ToString();
            var SearchEmployeeName = query.SearchEmployeeName;
            var GenderId = query.GenderId.ToString();
            var PositionId = query.PositionId.ToString();
            var SeniorityId = query.SeniorityId.ToString();
            var Sort = query.Sort.ToString();

            request.AddQueryParameter(nameof(SearchEmployeeName), SearchEmployeeName);
            request.AddQueryParameter(nameof(GenderId), GenderId);
            request.AddQueryParameter(nameof(DepartmentId), DepartmentId);
            request.AddQueryParameter(nameof(PositionId), PositionId);
            request.AddQueryParameter(nameof(SeniorityId), SeniorityId);
            request.AddQueryParameter(nameof(Sort), Sort);

            var response = await client.ExecuteGetAsync<EmployeeTableModel>(request);

            if (response.IsSuccessful)
            {
                var employees = response.Data;
                return View(employees);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Options()
        {
            var request = new RestRequestBuilder("/api/employees/options", GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<EmployeeTableFilterOptions>(request);

            if (response.IsSuccessful)
            {
                var options = response.Data;
                return Ok(options);
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var request = new RestRequestBuilder("/api/employees/getCreate", GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<EmployeeCreateRequestModel>(request);

            if (response.IsSuccessful)
            {
                var data = response.Data;
                return View(data);
            }

            return RedirectToAction("All");
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateResponseModel requestModel)
        {
            var request = new RestRequestBuilder("/api/employees/postCreate", GetAuthenticationClaim())
                .SetMethod(Method.Post)
                .AddBody(requestModel)
                .AddAuthentication()
                .Build();

            var response = await client.ExecutePostAsync<bool>(request);

            if (response.Data)
            {
                return RedirectToAction("All");
            }

            return RedirectToAction("Create", "Employee");
        }

        [HttpPost("employees/postCreate")]
        public async Task<IActionResult> CreateEmployeeFromFile(IFormFile file)
        {
            var getFileExtension = Path.GetExtension(file.FileName);
            var convertIntoByteArr = await FileHelper.ReadAsByteArrAsync(file);

            var fileToSend = new EmployeeFileInternalDTO
            {
                File = convertIntoByteArr,
                Extension = getFileExtension
            };

            if (fileToSend.File.Length == 0)
            {
                return BadRequest("Empty file");
            }

            var request = new RestRequestBuilder("/api/employees/postCreateFile",
                    GetAuthenticationClaim())
                .SetMethod(Method.Post)
                .AddAuthentication()
                .AddBody(fileToSend)
                .Build();

            var response = await client.ExecutePostAsync<string>(request);

            if (response.IsSuccessful)
            {
                return Created("", response.Content);
            }

            return BadRequest(response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            var request = new RestRequestBuilder($"/api/employees/edit/{id}",
                    GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<EmployeeGetEditModel>(request);

            if (response.IsSuccessful)
            {
                return View(response.Data);
            }

            return RedirectToAction("All", "Employee");
        }

        [HttpPut("employees/edit/{employeeId}")]
        public async Task<IActionResult> Edit(string employeeId, EmployeeSendEditModel model)
        {
            var request = new RestRequestBuilder($"/employees/edit/{employeeId}",
                    GetAuthenticationClaim())
                .SetMethod(Method.Put)
                .AddAuthentication()
                .AddBody(model)
                .Build();

            var response = await client.ExecutePutAsync<string>(request);
            if (response.IsSuccessful)
            {
                return Ok(response.Content);
            }

            return BadRequest();
        }

        [HttpGet("employees/search")]
        public async Task<IActionResult> GetEmployeesWithNoDepartmentsByName(string? name)
        {
            if (name == null)
            {
                name = "undefined";
            }

            var request = new RestRequestBuilder($"/api/employees/search/name/{name}", GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<ICollection<EmployeeSearchModel>>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return NotFound();
        }

        [HttpPut("employees/position/seniority")]
        public async Task<IActionResult> EditEmployeesPositionAndSeniority([FromBody]EmployeeEditPositionAndSeniority model)
        {
            var request = new RestRequestBuilder("/api/employees/edit/position/seniority", GetAuthenticationClaim())
                .SetMethod(Method.Put)
                .AddBody(model)
                .Build();

            var response = await client.ExecutePutAsync<string>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Data);
        }
    }
}