namespace Human_Capital_Managment.ViewModels.UserDetailViewModels
{
    using System.ComponentModel.DataAnnotations;

    using Huamn_Capital_Management.Constants.User_Details;

    public class UserDetailsResponseModel
    {
        [Required] public string EmployeeId { get; set; } = null!;

        [Required]
        [StringLength(UserDetailsConstants.PhoneNumberLength,MinimumLength = UserDetailsConstants.PhoneNumberLength)]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public int CountryOfBirth { get; set; }
        [Required]
        public int CountryOfResidenceId { get; set; }
        [Required]
        public int GenderId { get; set; }
    }
}
