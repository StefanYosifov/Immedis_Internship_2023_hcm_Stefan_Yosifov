namespace HCM.Models.ViewModels.Payments.Payroll
{
    using Bonuses;

    public class PayrollModel
    {
        public PayrollModel()
        {

            this.Bonuses = new HashSet<BonusModel>();
            this.Deductions = new HashSet<DeductionModel>();
        }

        public int Id { get; set; }
        public string EmployeeId { get; set; } = null!;

        public string EmployeeName { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Salary { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal DeductionAmount { get; set; }
        public decimal GrossPay { get; set; }
        public decimal NetPay { get; set; }
        public decimal TaxRate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? PaidOn { get; set; }
        public ICollection<BonusModel> Bonuses { get; set; }
        public ICollection<DeductionModel> Deductions { get; set; }
    }
}
