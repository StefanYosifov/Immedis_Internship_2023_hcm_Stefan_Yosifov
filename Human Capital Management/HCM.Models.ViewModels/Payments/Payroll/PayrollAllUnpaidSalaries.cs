namespace HCM.Models.ViewModels.Payments.Payroll
{
    public class PayrollAllUnpaidSalaries
    {

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = null!;

        public string? DepartmentImageUrl { get; set; }

        public decimal TotalPayment { get; set; }

        public int EmployeeCount { get; set; }

    }
}
