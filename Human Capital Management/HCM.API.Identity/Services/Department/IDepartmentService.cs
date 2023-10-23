namespace HCM.API.Services.Services.Department
{
    using Models.ViewModels.Departments;
    using Models.ViewModels.Positions;
    using Models.ViewModels.Seniorities;

    public interface IDepartmentService
    {
        Task<ICollection<DepartmentViewModel>> GetDepartments();

        Task<ICollection<PositionViewModel>> GetPositionsByDepartmentId(int id);

        Task<ICollection<SeniorityViewModel>> GetSenioritiesByPositionId(int id);
    }
}
