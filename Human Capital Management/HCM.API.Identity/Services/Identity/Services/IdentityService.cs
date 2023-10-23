namespace HCM.API.Services.Services.Identity.Services
{
    using Data.Models;

    using HCM.Models.ViewModels.Identity;

    using Microsoft.EntityFrameworkCore;

    using ApplicationDbContext = Data.ApplicationDbContext;

    public class IdentityService : IIdentityService
    {
        private readonly ApplicationDbContext context;

        public IdentityService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Employee> SignIn(LoginViewModel model)
        {
            var findEmployee = await context.Employees.FirstOrDefaultAsync(e =>
                e.Email == model.LoginParameter || e.Username == model.LoginParameter);

            if (findEmployee == null)
            {
                return null;
            }

            var verifyPasswordMatch = BCrypt.Net.BCrypt.Verify(model.Password, findEmployee.PasswordHash);

            if (verifyPasswordMatch == false)
            {
                return null;
            }

            return findEmployee;
        }

    }
}
