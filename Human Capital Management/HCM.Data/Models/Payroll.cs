namespace HCM.Data.Models
{
    using History_and_Audit;

    public class Payroll : IDeletableEntity
    {
        public Payroll()
        {
            BonusesNavigation = new HashSet<Bonuse>();
            DeductionsNavigation = new HashSet<Deduction>();
        }

        public int Id { get; set; }
        public string EmployeeId { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Salary { get; set; }
        public decimal Bonuses { get; set; }
        public decimal Deductions { get; set; }
        public decimal GrossPay { get; set; }
        public decimal NetPay { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? PaidOn { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }

        public virtual Employee Employee { get; set; } = null!;
        public virtual ICollection<Bonuse> BonusesNavigation { get; set; }
        public virtual ICollection<Deduction> DeductionsNavigation { get; set; }
    }
}