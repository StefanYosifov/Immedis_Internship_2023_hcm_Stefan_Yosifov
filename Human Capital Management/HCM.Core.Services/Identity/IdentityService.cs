namespace HCM.Core.Services.Identity
{
    using Common.Constants;
    using Common.Manager;
    using Data.Models;
    using HCM.Models.ViewModels.Identity;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    using Microsoft.Extensions.Configuration;

    using ApplicationDbContext = Data.ApplicationDbContext;

    public class IdentityService : IIdentityService
    {
        private readonly ApplicationDbContext context;
        private readonly IEmployeeManager employeeManager;
        private readonly IConfiguration configuration;

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
            var findEmployee = await employeeManager.FindEmployeeByUsernameOrPassword(model.LoginParameter);

            if (findEmployee == null)
            {
                return null;
            }

            var verifyPasswordMatch = BCrypt.Net.BCrypt
                .Verify(model.Password, findEmployee.PasswordHash);

            if (verifyPasswordMatch == false)
            {
                return null;
            }

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, findEmployee.Username),
                new(ClaimTypes.NameIdentifier, findEmployee.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var employeeRoles = findEmployee.EmployeeRoles
                .Select(er => er.Role.Name).ToArray();

            foreach (var role in employeeRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = GetToken(authClaims);

            return GetResponse(token, findEmployee, authClaims);
        }




        private Response GetResponse(JwtSecurityToken token, Employee user, IList<Claim> claims)
        {
            var response = new Response();

            response.JwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            response.Employee = user;
            foreach (var claim in claims)
            {
                response.Claims[claim.Type] = claim.Value;
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
