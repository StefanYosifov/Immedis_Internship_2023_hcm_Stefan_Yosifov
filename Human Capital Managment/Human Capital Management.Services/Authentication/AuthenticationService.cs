namespace Human_Capital_Management.Services.Authentication
{
    using Human_Capital_Managment.Data;
    using Human_Capital_Managment.Data.Models;
    using Human_Capital_Managment.ViewModels.AuthenticationViewModels;
    using Microsoft.EntityFrameworkCore;
    using System.Net;
    using System.Security.Claims;



    public class AuthenticationService:IAuthenticationService
    {
        private readonly HumanCapitalManagementContext context;

        
        public AuthenticationService(
            HumanCapitalManagementContext context)
        {
            this.context = context;
            
        }


        public async Task<bool> Register(RegisterViewModel registerModel)
        {
            var findEmployee = await FindEmployeeByEmail(registerModel.Email);

            if (findEmployee == null)
            {
                return false;
            }

            
        }

        public Task<bool> Login(LoginViewModel loginModel)
        {
            throw new NotImplementedException();
        }


        private async Task SignInUser(string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("MyCustomClaim", "my claim value")
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, Cookie);

            await HttpContex

            
        }

        private async Task<Employee?> FindEmployeeByEmail(string email)
        {
            return await this.context.Employees
                .FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower());
        }
    }
}
