namespace HCM.Common.Manager
{
    using System.Security.Claims;

    using HCM.Data;
    using HCM.Data.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class EmployeeManager : IEmployeeManager
    {

        private readonly ClaimsPrincipal user;
        private readonly ApplicationDbContext context;

        public EmployeeManager(IHttpContextAccessor httpContext,
            ApplicationDbContext context)
        {
            this.context = context;
            user = httpContext.HttpContext!.User;
        }


        public async Task<Employee> GetEmployee()
        {
            string employeeId = GetUserId();
            return (await context.Employees.FindAsync(employeeId))!;
        }

        public async Task<Employee?> FindEmployeeByEmail(string email)
        {
            return (await context.Employees
                .FirstOrDefaultAsync(e => e.Email == email));
        }

        public async Task<Employee?> FindEmployeeByUsername(string username)
        {
            return (await context.Employees
                .FirstOrDefaultAsync(e => string.Equals(e.Username, username, StringComparison.InvariantCultureIgnoreCase)));
        }

        public async Task<Employee?> FindEmployeeByUsernameOrPassword(string data)
        {
            return await context.Employees.FirstOrDefaultAsync(e =>
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
    }

}
