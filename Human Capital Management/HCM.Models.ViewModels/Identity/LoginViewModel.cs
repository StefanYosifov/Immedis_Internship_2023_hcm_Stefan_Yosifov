namespace HCM.Models.ViewModels.Identity
{
    using System.ComponentModel.DataAnnotations;

    public class LoginViewModel
    {
        [Required]
        public string LoginParameter { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}