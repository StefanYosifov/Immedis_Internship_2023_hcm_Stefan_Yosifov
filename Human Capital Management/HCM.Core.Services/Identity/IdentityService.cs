namespace HCM.Core.Services.Identity
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    using BCrypt.Net;

    using Common.Constants;
    using Common.Manager;

    using Data;
    using Data.Models;

    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    using Models.ViewModels.Identity;

    public class IdentityService : IIdentityService
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;
        private readonly IEmployeeManager employeeManager;

        public IdentityService(
            ApplicationDbContext context,
            IEmployeeManager employeeManager,
            IConfiguration configuration)
        {
            this.context = context;
            this.employeeManager = employeeManager;
            this.configuration = configuration;
        }

        public async Task<Response> SignIn(LoginViewModel model)
        {
            var findEmployee = await employeeManager
                .FindEmployeeByUsernameOrPassword(model.LoginParameter);

            if (findEmployee == null)
            {
                return null;
            }

            var verifyPasswordMatch = BCrypt
                .Verify(model.Password, findEmployee.PasswordHash);

            if (verifyPasswordMatch == false)
            {
                return null;
            }

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, findEmployee.Username),
                new(ClaimTypes.NameIdentifier, findEmployee.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var employeeRoles = findEmployee.EmployeeRoles
                .Select(er => er.Role.Name)
                .ToArray();

            foreach (var role in employeeRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            JwtSecurityToken token = GetToken(authClaims);

            Response response= GetResponse(token, findEmployee);

            return response;
        }

        public async Task<string> ChangePassword(ChangePasswordModel model)
        {
            var employee = await employeeManager.GetEmployee();

            var verifyOldPassword = BCrypt.Verify(model.OldPassword, employee!.PasswordHash);

            if (verifyOldPassword == false)
            {
                throw new InvalidOperationException("Old password is incorrect");
            }

            var newPasswordHash = BCrypt.HashPassword(model.NewPassword);

            employee.PasswordHash = newPasswordHash;
            await context.SaveChangesAsync();

            return "You have successfully change your password";
        }

        private Response GetResponse(JwtSecurityToken token, Employee user)
        {
            var response = new Response
            {
                JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                Employee = new EmployeeResponse
                {
                    Username = user.Username,
                    Email = user.Email,
                    Id = user.Id
                }
            };

            foreach (var role in user.EmployeeRoles)
            {
                response.Employee.Roles.Add(role.Role.Name);
            }

            return response;
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                configuration["JWT:ValidIssuer"],
                configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(JwtAuthenticationConstants.IdentityTokenHoursExpirationHours),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}