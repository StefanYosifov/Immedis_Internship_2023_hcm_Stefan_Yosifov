namespace HCM.Models.ViewModels.Employees
{
    using System.ComponentModel.DataAnnotations;

    public class EmployeeCreateRequestModel
    {

        public string Id { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public EmployeeCreateDropDownOptions Options { get; set; } = null!;
    }
}
