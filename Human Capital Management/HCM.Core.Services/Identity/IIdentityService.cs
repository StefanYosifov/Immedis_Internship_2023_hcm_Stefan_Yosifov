namespace HCM.Core.Services.Identity
{
    using Data.Models;

    using Models.ViewModels.Identity;

    public interface IIdentityService
    {
        Task<Response> SignIn(LoginViewModel model);

    }
}
