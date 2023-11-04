namespace HCM.Core.Services.Details
{
    using Data;

    using Microsoft.EntityFrameworkCore;

    internal class DetailsService : IDetailsService
    {
        private readonly ApplicationDbContext context;

        public DetailsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<decimal?> GetAverageSalaryInDepartmentById(int id)
        {
            return await context.Employees
                .Select(e => new
                {
                    e.DepartmentId,
                    e.Salary!.SalaryAmount
                })
                .Where(e => e.DepartmentId == id)
                .Select(e => e.SalaryAmount)
                .AverageAsync();
        }

        public async Task<decimal?> GetAverageSalaryInPositionById(int id)
        {
            return await context.Employees
                .Select(e => new
                {
                    e.PositionId,
                    e.Salary!.SalaryAmount
                })
                .Where(e => e.PositionId == id)
                .Select(e => e.SalaryAmount)
                .AverageAsync();
        }

        public async Task<decimal?> GetAverageSalaryInSeniorityById(int id)
        {
            return await context.Employees
                .Select(e => new
                {
                    e.SeniorityId,
                    e.Salary!.SalaryAmount
                })
                .Where(e => e.SeniorityId == id)
                .Select(e => e.SalaryAmount)
                .AverageAsync();
        }
    }
}