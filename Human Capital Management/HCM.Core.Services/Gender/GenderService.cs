namespace HCM.Core.Services.Gender
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Models.ViewModels.Genders;

    public class GenderService:IGenderService
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenderService(
            ApplicationDbContext context, 
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ICollection<GenderViewModel>> GetGenders()
        {
            return await context.Genders
                .ProjectTo<GenderViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }
    }
}
