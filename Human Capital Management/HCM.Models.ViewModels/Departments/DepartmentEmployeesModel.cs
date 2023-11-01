namespace HCM.Models.ViewModels.Departments
{
    public class DepartmentEmployeesModel
    {


        public string? EmployeeId { get; set; } = null!;

        public string? EmployeeFirstname { get; set; }

        public string? EmployeeLastname { get; set; } = null!;

        public string? EmployeeFullName => $"{EmployeeFirstname} {EmployeeLastname}";

        public int EmployeeAge { get; set; }

        public string? EmployeeNationalityISO { get; set; }

        public string? EmployeeGender { get; set; }

        public string? EmployeePosition { get; set; }

        public string? EmployeeSeniority { get; set; }
    }
}
