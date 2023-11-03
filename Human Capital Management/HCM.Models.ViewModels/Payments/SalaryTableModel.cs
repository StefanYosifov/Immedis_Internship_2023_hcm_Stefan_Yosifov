namespace HCM.Models.ViewModels.Payments
{
    public class SalaryTableModel
    {
        public string EmployeeId { get; set; } = null!;

        public string EmployeeFirstname { get; set; } = null!;

        public string EmployeeLastname { get; set; } = null!;

        public string EmployeeFullname => $"{EmployeeFirstname} {EmployeeLastname}";

        public int Age { get; set; }

        public string SalaryId { get; set; } = null!;

        public decimal? SalaryAmount { get; set; }

        public byte? GenderId { get; set; }

        public string? DepartmentName { get; set; }

        public string? PositionName { get; set; }

        public string? SeniorityName { get; set; } 

    }
}
