namespace HCM.Models.ViewModels.Employees
{
    public class EmployeeTableDataModel
    {
        public string EmployeeId { get; set; } = null!;

        public string EmployeeUserName { get; set; } = null!;

        public string EmployeeFirstName { get; set; } = null!;

        public string EmployeeLastName { get; set; } = null!;

        public int EmployeeAge { get; set; } 

        public string? EmployeeDepartmentName { get; set; } 
        
        public string? EmployeePositionName { get; set; } 

        public string? EmployeeSeniority { get; set; } 
    }
}
