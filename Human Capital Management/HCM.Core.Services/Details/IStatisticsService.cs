namespace HCM.Core.Services.Details
{
    using Models.ViewModels.Statistics;

    public interface IStatisticsService
    {
        Task<decimal?> GetAverageSalaryInDepartmentById(int id);
        Task<decimal?> GetAverageSalaryInPositionById(int id);
        Task<decimal?> GetAverageSalaryInSeniorityById(int id);
        Task<HomePageStatisticsModel> GetHomeStatistics();
    }
}