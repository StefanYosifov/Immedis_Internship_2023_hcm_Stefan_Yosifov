namespace HCM.API.Controllers.Country
{
    using Core.Services.Countries;

    using Microsoft.AspNetCore.Mvc;

    [Route("countries")]
    public class CountryController : ApiController
    {
        private readonly ICountryService service;

        public CountryController(ICountryService service)
        {
            this.service = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await service.GetCountries();

            return Ok(countries);
        }
    }
}