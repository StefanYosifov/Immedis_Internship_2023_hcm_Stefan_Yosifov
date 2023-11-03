namespace HCM.Models.ViewModels.Employees
{
    public class EmployeeSearchModel
    {
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string FullName => $"{FirstName} {LastName}";

        public int Age { get; set; }

        public DateTime AccountCreatedOn { get; set; }
    }
}