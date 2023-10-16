namespace Human_Capital_Management.Services.UserDetails
{
    using Human_Capital_Managment.ViewModels.UserDetailViewModels;

    public interface IUserDetailsService
    {

        Task<UserDetailsRequestModels> GetUserDetailsViewModelOptions();

        Task<bool> SaveUserDetailsOptions(UserDetailsResponseModel registerModel,string userId);
    }
}
