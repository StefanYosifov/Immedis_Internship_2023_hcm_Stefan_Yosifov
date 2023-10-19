﻿namespace HCM.API.Identity.Identity.Services
{
    using API.Services.Services.Identity.Services;

    using Data;
    using Data.Models;

    using Microsoft.EntityFrameworkCore;

    using Models.ViewModels.Identity;

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