namespace HCM.Models.ViewModels.Payments
{
    public class SalaryChangeModel
    {
        public string EmployeeId { get; set; } = null!;

        public string EmployeeFullName { get; set; } = null!;

        public int TimeInCompany { get; set; }

        public int Age { get; set; }

        public int? DepartmentId { get; set; }

        public string DepartmentName { get; set; } = null!;

        public decimal? AverageDepartmentSalary { get; set; }

        public int? PositionId { get; set; }

        public string PositionName { get; set; } = null!;

        public decimal? AveragePositionSalary { get; set; }

        public int? SeniorityId { get; set; }

        public string SeniorityName { get; set; } = null!;

        public decimal? AverageSenioritySalary { get; set; }

        public decimal? CurrentSalary { get; set; }
    }
}