// ReSharper disable InconsistentNaming
namespace HCM.Controllers.Employee
{
    using Common.Requests;

    using HCM.Common.Constants;
    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Employees;

    using RestSharp;

    public class EmployeeController : Controller
    {
        private readonly RestClient client;

        public EmployeeController()
        {
            this.client = new RestClient(ApplicationAPIConstants.API_BASE_URL);
        }

        [HttpGet]
        public async Task<IActionResult> All(int id, [FromQuery] EmployeeQueryTableFilters query)
        {
            var request = new RestRequest($"/employees/{id}");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authentication", $"Bearer {Request.Headers["Authentication"]}");

            string? DepartmentId = query.DepartmentId.ToString();
            string? SearchEmployeeName = query.SearchEmployeeName;
            string? GenderId = query.GenderId.ToString();
            string? PositionId = query.PositionId.ToString();
            string? SeniorityId = query.SeniorityId.ToString();
            string? Sort = query.Sort.ToString();

            request.AddQueryParameter(nameof(SearchEmployeeName), SearchEmployeeName);
            request.AddQueryParameter(nameof(GenderId), GenderId);
            request.AddQueryParameter(nameof(DepartmentId), DepartmentId);
            request.AddQueryParameter(nameof(PositionId), PositionId);
            request.AddQueryParameter(nameof(SeniorityId), SeniorityId);
            request.AddQueryParameter(nameof(Sort), Sort);


            var response = await client.ExecuteAsync<EmployeeTableModel>(request);

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
            var request = new RestRequest($"/employees/options");
            request.AddHeader("Accept", "application/json");

            var response = await client.ExecuteAsync<EmployeeTableFilterOptions>(request);

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
            var request = new RestRequest("/employees/getCreate");
            request.AddHeader("Accept", "application/json");

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
            var request = new RestRequest("/employees/postCreate", Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddBody(requestModel);

            var response = await client.ExecuteAsync<bool>(request);

            if (response.Data == true)
            {
                return RedirectToAction("All");
            }

            return RedirectToAction("Create", "Employee");
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute]string id)
        {

            var request = new RestRequestBuilder($"/employees/edit/{id}")
                .SetMethod(Method.Get)
                .Build();


            var response = await this.client.ExecuteGetAsync<EmployeeGetEditModel>(request);

            if (response.IsSuccessful)
            {
                return View(response.Data);
            }

            return RedirectToAction("All", "Employee");
        }
    }
}
