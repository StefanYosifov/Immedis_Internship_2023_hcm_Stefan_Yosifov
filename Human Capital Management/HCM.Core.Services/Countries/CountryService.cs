namespace HCM.Core.Services.Countries
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Models.ViewModels.Countries;

    internal class CountryService : ICountryService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CountryService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ICollection<CountryViewModel>> GetCountries()
        {
            return await context.Countries
                .ProjectTo<CountryViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public async Task<Dictionary<int, decimal?>> GetCountriesTaxRates()
        {
            return await context.Countries
                .Select(c => new
                {
                    c.Id,
                    c.TaxRate
                })
                .ToDictionaryAsync(entry => entry.Id, entry => entry.TaxRate);
        }
    }
}