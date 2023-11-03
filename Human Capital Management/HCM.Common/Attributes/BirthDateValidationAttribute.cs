namespace HCM.Common.Attributes;

using System.ComponentModel.DataAnnotations;

using Constants;

public class BirthDateValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime)
        {
            var birthdate = Convert.ToDateTime(value);

            var age = DateTime.Today.Year - birthdate.Year;

            if (age < ValidationConstants.EmployeeConstants.MinEmployeeAge)
            {
                return new ValidationResult($"The employee you are trying to register must be at least " +
                                            $"{ValidationConstants.EmployeeConstants.MinEmployeeAge} years old");
            }

            if (age > ValidationConstants.EmployeeConstants.MaxEmployeeAge)
            {
                return new ValidationResult($"The employee you are trying to register must be below " +
                                            $"{ValidationConstants.EmployeeConstants.MaxEmployeeAge} years old");
            }

            return ValidationResult.Success!;
        }

        return new ValidationResult("Date of birth may contain only a valid date.");
    }
}