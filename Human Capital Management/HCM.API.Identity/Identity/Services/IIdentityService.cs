namespace HCM.API.Identity.Identity.Services
{
    using Data.Models;

    using Models.ViewModels.Identity;

    public interface IIdentityService
    {
        Task<Employee> SignIn(LoginViewModel model);

    }
}
