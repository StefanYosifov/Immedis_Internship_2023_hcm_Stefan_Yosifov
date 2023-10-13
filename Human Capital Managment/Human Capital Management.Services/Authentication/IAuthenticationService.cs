namespace Human_Capital_Management.Services.Authentication
{
    using Human_Capital_Managment.ViewModels.AuthenticationViewModels;

    public interface IAuthenticationService
    {

        Task<bool> Register(RegisterViewModel registerModel);

        Task<bool> Login(LoginViewModel loginModel);

    }
}
