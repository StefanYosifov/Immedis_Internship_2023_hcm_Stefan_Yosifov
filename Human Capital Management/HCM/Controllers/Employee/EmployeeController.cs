namespace HCM.Controllers.Employee
{
    using Common;

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
        public async Task<IActionResult> Index(int id, [FromQuery]EmployeeQueryTableFilters query)
        {
            var request = new RestRequest($"/employees/{id}");
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(query);

            var response = await client.ExecuteAsync<IList<EmployeeTableModel>>(request);

            if (response.IsSuccessful)
            {
                var employees = response.Data;
                return View(employees);
            }

            return BadRequest();
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
            var request = new RestRequest("/employees/create");
            request.AddHeader("Accept", "application/json");

            var response = await client.ExecuteGetAsync<EmployeeCreateModel>(request);

            if (response.IsSuccessful)
            {
                var data = response.Data;
                return View(data);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id)
        {
            return null;

        }
    }
}
