namespace HCM.Controllers.Payments
{
    using Common.Requests;
    using Microsoft.AspNetCore.Mvc;
    using Models.ViewModels.Payments;
    using Models.ViewModels.Payments.Bonuses;
    using RestSharp;
    using System.Globalization;
    using System.Net;

    public class PaymentController : BaseController
    {
        private const string DefaultRoute = "/payments";

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
        public async Task<IActionResult> GetEmployeesTableInformation(int page, [FromQuery] SalaryTableQueryModel query)
        {
            var request = new RestRequestBuilder($"/api/payments/salary/all/{page}", GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddHeader("Content-Type", "application/json")
                .AddAuthentication()
                .Build();

            var DepartmentId = query.DepartmentId.ToString();
            var Search = query.Search;
            var PositionId = query.PositionId.ToString();
            var SeniorityId = query.SeniorityId.ToString();
            var Sort = query.Sort.ToString();

            request.AddQueryParameter(nameof(Search), Search);
            request.AddQueryParameter(nameof(DepartmentId), DepartmentId);
            request.AddQueryParameter(nameof(PositionId), PositionId);
            request.AddQueryParameter(nameof(Sort), Sort);
            request.AddQueryParameter(nameof(SeniorityId), SeniorityId);

            var response = await client.ExecuteGetAsync<SalaryTablePagination>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Data);
        }

        public async Task<IActionResult> Details(string id)
        {
            var request = new RestRequestBuilder($"/api/payments/salary/info/{id}", GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<SalaryChangeModel>(request);

            if (response.IsSuccessful)
            {
                return View(response.Data);
            }

            return RedirectToAction("GetEmployeesTableInformation", "Payment");
        }

        [HttpGet("payments/bonus/reasons")]
        public async Task<IActionResult> GetAllBonusReasons()
        {
            var request = new RestRequestBuilder("/api/payments/bonus/reasons", GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<ICollection<BonusReasonModel>>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Data);
        }

        [HttpGet("payments/deduction/reasons")]
        public async Task<IActionResult> GetAllDeductionReasons()
        {
            var request = new RestRequestBuilder("/api/payments/deduction/reasons", GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<ICollection<DeductionReasonModel>>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Data);
        }

        [HttpGet("payments/monthly/additions")]
        public async Task<IActionResult> GetMonthlySalaryAdditionsByMonth(
            [FromQuery] TableBonusDeductionSearchModel model)
        {
            if (string.IsNullOrEmpty(model.MonthYearOfSearch))
            {
                model.MonthYearOfSearch = DateTime.UtcNow.ToString("dd/mm/yyyy").Replace('.', '-');
            }

            var request = new RestRequestBuilder("/api/payments/monthly/additions", GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddQueryParameter(model)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<MonthlyBonusDeductionTableModel>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Data);
        }

        [HttpPut("payments/salary/change")]
        public async Task<IActionResult> ChangeSalary([FromBody] SalaryChangeRequestModel model)
        {
            var request = new RestRequestBuilder("/api/payments/salary/change", GetAuthenticationClaim())
                .SetMethod(Method.Put)
                .AddQueryParameter(model)
                .AddAuthentication()
                .Build();

            var response = await client.ExecutePutAsync<string>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(response.Data);
            }

            return BadRequest(response.Data);

        }

        [HttpPost("payments/bonus/add")]
        public async Task<IActionResult> AddBonus([FromBody] BonusAddModel model)
        {
            var request = new RestRequestBuilder("/api/payments/bonus/add", GetAuthenticationClaim())
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

        [HttpPost("payments/deduction/add")]
        public async Task<IActionResult> AddDeduction([FromBody] DeductionAddModel model)
        {
            var request = new RestRequestBuilder("/api/payments/deduction/add", GetAuthenticationClaim())
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