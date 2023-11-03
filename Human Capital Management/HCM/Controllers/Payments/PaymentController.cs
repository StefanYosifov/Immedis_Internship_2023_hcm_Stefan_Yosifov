namespace HCM.Controllers.Payments
{
    using Common.Requests;
    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Payments;

    using RestSharp;

    using Method = RestSharp.Method;

    public class PaymentController : BaseController
    {
        private const string DefaultRoute = "/payments";
        private const string DefaultApiRoute = "/api/payments";

        public IActionResult Salary()
        {

            return View();
        }

        [HttpGet("/payments/salary/options")]
        public async Task<IActionResult> GetDropDownsForSalaries()
        {
            var request = new RestRequestBuilder("api/payments/salary/options", GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<SalaryTableQueryOptionsModel>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("/payments/salary/all/{page}")]
        public async Task<IActionResult> GetEmployeesSalaryInformation(int page, [FromQuery] SalaryTableQueryModel query)
        {
            var request = new RestRequestBuilder($"/api/payments/salary/all/{page}", GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddHeader("Content-Type", "application/json")
                .AddQuery(query.Search, query.DepartmentId.ToString(), query.PositionId.ToString(), query.SeniorityId.ToString(), query.Sort.ToString())
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<SalaryTablePagination>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Data);
        }

    }
}
