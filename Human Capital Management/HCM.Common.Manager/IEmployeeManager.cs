namespace HCM.Common.Manager
{
    using Data.Models;

    using Models.ViewModels.Roles;

    public interface IEmployeeManager
    {
        public Task<Employee?> GetEmployee();

        public Task<Employee?> FindEmployeeByEmail(string email);

        public Task<Employee?> FindEmployeeByUsername(string username);

        public Task<Employee?> FindEmployeeByUsernameOrPassword(string data);

        string GetUserId();

        string GetUserName();

        string GetJwtToken();

        bool IsInRole(RolesEnum rolesEnum);
    }
}