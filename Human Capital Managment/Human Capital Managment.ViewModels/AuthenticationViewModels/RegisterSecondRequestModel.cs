namespace Human_Capital_Managment.ViewModels.AuthenticationViewModels
{
    using System.ComponentModel.DataAnnotations;

    using UserDetailViewModels;

    public class RegisterSecondRequestModel
    {
        [Required] public RegisterViewModel RegisterViewModel { get; set; } = null!;

        [Required]
        public UserDetailsRequestModels UserDetailsRequest { get; set; } = null!;

    }
}
