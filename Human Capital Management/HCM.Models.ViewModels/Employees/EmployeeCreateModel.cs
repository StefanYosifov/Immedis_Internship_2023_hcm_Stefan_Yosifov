namespace HCM.Models.ViewModels.Employees
{
    public class EmployeeCreateModel
    {

        public string Id { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; } = null!;

        public EmployeeCreateDropDownOptions Options { get; set; } = null!;
    }
}
