namespace HCM.Core.Services.Department
{
    using Models.ViewModels.Departments;
    using Models.ViewModels.Positions;
    using Models.ViewModels.Seniorities;

    public interface IDepartmentService
    {
        Task<ICollection<DepartmentViewModel>> GetDepartments();

        Task<ICollection<PositionViewModel>> GetPositionsByDepartmentId(int id);

        Task<ICollection<SeniorityViewModel>> GetSenioritiesByPositionId(int id);

        Task<ICollection<DepartmentGetAllModel>> GetAllDepartments(DepartmentSendQueryFilters query);

        Task<DepartmentGetAllQueryFilters> GetAllQueryFilters();

        Task<DepartmentDetailsViewModel> GetDepartmentDetailsById(int id);

        Task<ICollection<DepartmentGetPositionsModel>> GetPositionsInTheDepartmentById(int id);

        Task<ICollection<PositionViewModel>> GetAvailablePositionsToAddToDepartmentById(int id);

        Task<string> AddPositionToDepartmentById(DepartmentAddPosition model);

        Task<string> RemovePositionFromDepartmentById(DepartmentRemovePosition model);

        Task<string> AddEmployeeToDepartmentById(DepartmentAddEmployee model);

        Task<string> RemoveEmployeeFromDepartmentById(DepartmentRemoveEmployee model);

        Task<string> EditDepartmentDetails(DepartmentEditDetails model);
    }
}