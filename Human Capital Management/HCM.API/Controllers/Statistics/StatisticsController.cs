namespace HCM.API.Controllers.Statistics
{
    using Core.Services.Details;

    using Microsoft.AspNetCore.Mvc;

    [Route("/api/statistics")]
    public class StatisticsController : ApiController
    {
        private readonly IStatisticsService service;

        public StatisticsController(IStatisticsService service)
        {
            this.service = service;
        }

        [HttpGet("home")]
        public async Task<IActionResult> GetHomeStatistics()
        {
            var result = await service.GetHomeStatistics();
            return Ok(result);
        }
    }
}
