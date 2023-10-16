namespace Human_Capital_Managment.ViewModels.AuthenticationViewModels
{
    using System.ComponentModel.DataAnnotations;

    using UserDetailViewModels;

    public class RegisterSecondResponseModel
    {
        [Required] public RegisterViewModel RegisterViewModel { get; set; } = null!;

        [Required] public UserDetailsResponseModel UserDetailsViewModel { get; set; } = null!;
    }
}
