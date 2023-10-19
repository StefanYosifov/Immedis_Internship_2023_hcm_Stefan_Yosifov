namespace HCM.API.Services.Services.Identity.Services
{
    using Data.Models;

    using Models.ViewModels.Identity;

    public interface IIdentityService
    {
        Task<Employee> SignIn(LoginViewModel model);

    }
}
