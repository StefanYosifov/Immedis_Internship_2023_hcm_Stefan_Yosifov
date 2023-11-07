namespace HCM.Models.ViewModels.Employees
{
    public class EmployeeBirthdayModel
    {

        public string EmployeeId { get; set; } = null!;

        public string EmployeeName { get; set; } = null!;

        public string? BirthDate { get; set; }

        public int Age { get; set; }

    }
}
