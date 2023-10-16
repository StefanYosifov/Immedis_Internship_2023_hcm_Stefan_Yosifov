namespace Human_Capital_Management.Services.Authentication
{
    using Human_Capital_Managment.Data.Models;
    using Human_Capital_Managment.ViewModels.AuthenticationViewModels;

    public interface IAuthenticationService
    {

        Task<Employee?> Register(RegisterViewModel registerModel);

        Task<Employee?> Login(LoginViewModel loginModel);

        Task<Employee?> FindEmployeeByEmail(string email);

    }
}
