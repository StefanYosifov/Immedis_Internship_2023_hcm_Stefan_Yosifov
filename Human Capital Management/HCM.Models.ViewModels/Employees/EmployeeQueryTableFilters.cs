namespace HCM.Models.ViewModels.Employees
{
    using Enum;

    public class EmployeeQueryTableFilters
    {
        
        public string? SearchEmployeeName { get; set; }

        public byte GenderId { get; set; }

        public int DepartmentId { get; set; }

        public int PositionId { get; set; }

        public int SeniorityId { get; set; }

        public EmployeeTableSortEnum? Sort { get; set; }

    }
}
