namespace HCM.Data.Models
{
    using History_and_Audit;

    public class Bonuse : ICreationEntity
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public string? EmployeeId { get; set; }
        public int ReasonId { get; set; }
        public int? PayrollId { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual Payroll? Payroll { get; set; }
        public virtual BonusesReason? Reason { get; set; }

        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
    }
}