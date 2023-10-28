namespace HCM.Data.Models
{
    public partial class Deduction
    {
        public long Id { get; set; }
        public decimal? Amount { get; set; }
        public string? EmployeeId { get; set; }
        public int? ReasonId { get; set; }
        public int? PayrollId { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual Payroll? Payroll { get; set; }
        public virtual DeductionReason? Reason { get; set; }
    }
}
