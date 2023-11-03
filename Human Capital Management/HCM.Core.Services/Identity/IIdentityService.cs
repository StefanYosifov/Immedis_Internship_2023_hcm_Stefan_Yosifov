namespace HCM.Core.Services.Identity
{
    using Models.ViewModels.Identity;

    public interface IIdentityService
    {
        Task<Response> SignIn(LoginViewModel model);

        Task<string> ChangePassword(ChangePasswordModel model);
    }
}