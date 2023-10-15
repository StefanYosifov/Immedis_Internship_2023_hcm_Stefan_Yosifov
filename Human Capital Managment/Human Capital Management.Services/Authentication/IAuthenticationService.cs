namespace Human_Capital_Management.Services.Authentication
{
    using Human_Capital_Managment.Data.Models2;
    using Human_Capital_Managment.ViewModels.AuthenticationViewModels;

    public interface IAuthenticationService
    {

        Task<Employee?> Register(RegisterViewModel registerModel);

        Task<Employee?> Login(LoginViewModel loginModel);

    }
}
