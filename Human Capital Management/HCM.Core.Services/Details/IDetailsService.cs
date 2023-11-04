namespace HCM.Core.Services.Details
{
    public interface IDetailsService
    {
        Task<decimal?> GetAverageSalaryInDepartmentById(int id);
        Task<decimal?> GetAverageSalaryInPositionById(int id);
        Task<decimal?> GetAverageSalaryInSeniorityById(int id);
    }
}