namespace HCM.Common.Manager
{
    using System.Security.Claims;

    using Data;
    using Data.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    using Models.ViewModels.Roles;

    public class EmployeeManager : IEmployeeManager
    {
        private readonly ApplicationDbContext context;

        private readonly ClaimsPrincipal user;

        public EmployeeManager(IHttpContextAccessor httpContext,
            ApplicationDbContext context)
        {
            user = httpContext.HttpContext.User;
            this.context = context;
        }

        public async Task<Employee> GetEmployee()
        {
            var employeeId = GetUserId();
            return (await context.Employees.FindAsync(employeeId))!;
        }

        public async Task<Employee?> FindEmployeeByEmail(string email)
        {
            return await context.Employees
                .Include(e => e.EmployeeRoles)
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<Employee?> FindEmployeeByUsername(string username)
        {
            return await context.Employees
                .Include(e => e.EmployeeRoles)
                .FirstOrDefaultAsync(e =>
                    string.Equals(e.Username, username, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<Employee?> FindEmployeeByUsernameOrPassword(string data)
        {
            return await context.Employees
                .Include(e => e.EmployeeRoles)
                .ThenInclude(er => er.Role)
                .FirstOrDefaultAsync(e =>
                    e.Email == data || e.Username == data);
        }

        public string GetUserId()
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        string IEmployeeManager.GetUserName()
        {
            return user.FindFirstValue(ClaimTypes.Name);
        }

        public string GetJwtToken()
        {
            return user.FindFirstValue(ClaimTypes.Authentication);
        }

        public bool IsInRole(RolesEnum role)
        {
            return user.IsInRole(role.ToString());
        }
    }
}