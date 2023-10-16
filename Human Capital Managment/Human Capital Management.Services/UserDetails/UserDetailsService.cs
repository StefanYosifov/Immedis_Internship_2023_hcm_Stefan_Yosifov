namespace Human_Capital_Management.Services.UserDetails
{
    using System.Security.Claims;

    using AutoMapper;

    using Huamn_Capital_Managment.Common.Extensions;

    using Human_Capital_Managment.Data;
    using Human_Capital_Managment.Data.Models;
    using Human_Capital_Managment.ViewModels.AuthenticationViewModels;
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

        public async Task<bool> SaveUserDetailsOptions(UserDetailsResponseModel registerModel, string userId)
        {
            var findDetails = await context.EmployeeDetails.FindAsync(userId);

            if (findDetails == null)
            {
                var createdDetails = mapper.Map<EmployeeDetail>(registerModel);
                await context.EmployeeDetails.AddAsync(createdDetails);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }

    }
}
