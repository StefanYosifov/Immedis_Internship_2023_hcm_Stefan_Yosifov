namespace Human_Capital_Managment.ViewModels.Employee
{
    public class EmployeeViewModel
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Age { get; set; }
        public string DepartmentName { get; set; } = null!;
        public string PositionName { get; set; } = null!;
        public string SeniorityName { get; set; } = null!;

        public string Status { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string ContractId { get; set; } = null!;

    }
}
