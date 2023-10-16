namespace Human_Capital_Management.Services.UserDetails
{
    using AutoMapper;

    using Human_Capital_Managment.Data;
    using Human_Capital_Managment.ViewModels.UserDetailViewModels;

    using Microsoft.EntityFrameworkCore;

    public class UserDetailsService:IUserDetailsService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UserDetailsService(
            ApplicationDbContext context, 
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<UserDetailsRequestModels> GetUserDetailsViewModelOptions()
        {
            var getCountries = await context.Countries
                .ToArrayAsync();

            var getGenders=await context.Genders
                .ToArrayAsync();
            
            return new UserDetailsRequestModels()
            {
                CountriesOfBirth = getCountries,
                CountriesOfResidence = getCountries,
                Genders = getGenders,
            };

        }
    }
}
