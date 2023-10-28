namespace HCM.Data.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Payroll
    {
        public Payroll()
        {
            Bonuses = new HashSet<Bonuse>();
            DeductionsNavigation = new HashSet<Deduction>();
        }

        public int Id { get; set; }
        public string? EmployeeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Salary { get; set; }
        public decimal? Deductions { get; set; }
        public decimal? NetPay { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? PaidOn { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual ICollection<Bonuse> Bonuses { get; set; }
        public virtual ICollection<Deduction> DeductionsNavigation { get; set; }
    }
}
