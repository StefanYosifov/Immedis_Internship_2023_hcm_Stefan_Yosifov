namespace HCM.Models.ViewModels.Admin
{
    using Data.Models;

    public class AdminEmployeeModel
    {
        public AdminEmployeeModel()
        {
        }

        public string EmployeeId { get; set; } = null!;

        public string EmployeeUserName { get; set; } = null!;

        public string EmployeeFirstName { get; set; } = null!;

        public string EmployeeLastName { get; set; } = null!;

        public int EmployeeAge { get; set; }

        public string? EmployeeDepartmentName { get; set; }

        public string? EmployeePositionName { get; set; }

        public string? EmployeeSeniority { get; set; }

        public int? RoleId { get; set; }

    }


}
