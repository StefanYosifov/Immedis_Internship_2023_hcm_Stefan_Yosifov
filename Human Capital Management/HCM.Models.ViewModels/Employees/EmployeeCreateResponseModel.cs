namespace HCM.Models.ViewModels.Employees
{
    using Common.Constants;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EmployeeCreateResponseModel
    {

            [Required]
            [EmailAddress]
            public string Email { get; set; } = null!;

            [Required]
            [StringLength(ValidationConstants.EmployeeConstants.FirstNameMaxLength, MinimumLength = ValidationConstants.EmployeeConstants.FirstNameMinLength,
                ErrorMessage = "FirstName is too long or too short")]
            public string Firstname { get; set; } = null!;

            [Required]
            [StringLength(ValidationConstants.EmployeeConstants.LastNameMaxLength, MinimumLength = ValidationConstants.EmployeeConstants.LastNameMinLength,
                ErrorMessage = "LastName is too long or too short")]
            public string Lastname { get; set; } = null!;

            [Required]
            public DateTime BirthDate { get; set; }

            [Required]
            [Phone]
            public string PhoneNumber { get; set; } = null!;


            [Required]
            public int NationalityId { get; set; } 

            [Required]
            public byte GenderId { get; set; } 

            [Required]
            public int DepartmentId { get; set; }

            [Required]
            public int PositionId { get; set; }

            [Required]
            public int SeniorityId { get; set; }

        

    }
}
