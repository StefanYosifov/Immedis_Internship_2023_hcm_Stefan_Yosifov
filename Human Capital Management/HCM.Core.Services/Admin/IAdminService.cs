namespace HCM.Core.Services.Admin
{
    using Models.ViewModels.Admin;

    public interface IAdminService
    {

        Task<AuditLogsPagination> GetAuditLogs(int page);

        Task<AdminEmployeePagination> GetEmployees(AdminSearchEmployee model);

        Task<ICollection<RoleViewModel>> GetAllRoles();

        Task<string> ChangeEmployeeRole(AdminChangeRole model);

        Task<AdminDepartmentsCollection> GetDepartments();

        public Task<string> CreateDepartment(AdminCreateDepartment model);

    }
}
