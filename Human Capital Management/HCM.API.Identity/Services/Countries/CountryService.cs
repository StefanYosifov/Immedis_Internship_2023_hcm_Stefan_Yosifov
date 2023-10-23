namespace HCM.API.Services.Services.Countries
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Models.ViewModels.Countries;

    public class CountryService : ICountryService
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
    }
}
