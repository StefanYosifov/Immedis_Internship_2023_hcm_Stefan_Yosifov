namespace HCM.Controllers.Statistics
{
    using Common.Requests;

    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Statistics;

    using RestSharp;

    using Method = RestSharp.Method;

    public class StatisticsController : BaseController
    {

        [HttpGet("statistics/home")]
        public async Task<IActionResult> GetHomeStatistics()
        {
            var request = new RestRequestBuilder("/api/statistics/home", GetAuthenticationClaim())
                .SetMethod(Method.Get)
                .AddAuthentication()
                .Build();

            var response = await client.ExecuteGetAsync<HomePageStatisticsModel>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }

            return BadRequest();
        }

    }
}
