namespace HCM.Models.ViewModels.Employees
{
    using Enum;

    public class EmployeeQueryTableFilters
    {
        
        public string? SearchEmployeeName { get; set; }

        public byte? DepartmentId { get; set; }

        public byte? PositionId { get; set; }

        public byte? SeniorityId { get; set; }

        public EmployeeTableSortEnum? Sort { get; set; }

    }
}
