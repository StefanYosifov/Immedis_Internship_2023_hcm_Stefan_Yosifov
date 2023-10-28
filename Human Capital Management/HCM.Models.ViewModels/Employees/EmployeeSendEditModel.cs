namespace HCM.Models.ViewModels.Employees
{
    using System.ComponentModel.DataAnnotations;

    public class EmployeeSendEditModel
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public int NationalityId { get; set; }

        [Required]
        public byte DepartmentId { get; set; }

        [Required]
        public int PositionId { get; set; }

        [Required]
        public int SeniorityId { get; set; }

    }
}
