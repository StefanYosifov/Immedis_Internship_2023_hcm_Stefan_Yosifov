namespace HCM.Common.Manager
{
    using System.Runtime.CompilerServices;

    using Data.Models;

    public interface IEmployeeManager
    {

        public Task<Employee?> GetEmployee();

        public Task<Employee?> FindEmployeeByEmail(string email);

        public Task<Employee?> FindEmployeeByUsername(string username);

        public Task<Employee?> FindEmployeeByUsernameOrPassword(string data);

        string GetUserId();

        string GetUserName();
    }
}
