namespace HCM.Models.ViewModels.Identity
{
    using System.ComponentModel.DataAnnotations;

    public class ChangePasswordModel
    {
        [Required] public string OldPassword { get; set; } = null!;

        [Required] public string NewPassword { get; set; } = null!;
    }
}