namespace HCM.Core.Services.Admin
{
    using Models.ViewModels.Admin;

    public interface IAdminService
    {

        Task<AuditLogsPagination> GetAuditLogs(int page);

        Task<AdminEmployeePagination> GetEmployees(AdminSearchEmployee model);

        Task<ICollection<RoleViewModel>> GetAllRoles();

    }
}
