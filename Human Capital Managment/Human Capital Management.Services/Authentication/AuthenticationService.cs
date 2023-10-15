namespace Human_Capital_Management.Services.Authentication
{
    using AutoMapper;
    using Human_Capital_Managment.Data;
    using Human_Capital_Managment.Data.Models2;
    using Human_Capital_Managment.ViewModels.AuthenticationViewModels;
    using Microsoft.EntityFrameworkCore;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        private const int PasswordIterations = 15; // In real world, scale this up to a higher number

        public AuthenticationService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public async Task<Employee?> Register(RegisterViewModel registerModel)
        {
            var findEmployee = await FindEmployeeByEmail(registerModel.Email);

            if (findEmployee != null)
            {
                return null;
            }

            string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(registerModel.Password, PasswordIterations);

            var employee = mapper.Map<Employee>(registerModel);
            employee.PasswordHash = passwordHash;
            employee.JoinedAt=DateTime.UtcNow;

            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();

            return employee;
        }

        public async Task<Employee?> Login(LoginViewModel loginModel)
        {
            var findEmployee = await FindEmployeeByEmail(loginModel.Email);

            if (findEmployee == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password,findEmployee.PasswordHash))
            {
                return null;
            }

            return findEmployee;
        }

        private async Task<Employee?> FindEmployeeByEmail(string email)
        {
            return await this.context.Employees
                .FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower());
        }
    }
}
