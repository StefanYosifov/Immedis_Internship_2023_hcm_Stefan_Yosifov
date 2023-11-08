using System;
using System.Collections.Generic;

namespace HCM.API.molelzz
{
    public partial class Bonuse
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? EmployeeId { get; set; }
        public int? ReasonId { get; set; }
        public int? PayrollId { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual Payroll? Payroll { get; set; }
        public virtual BonusesReason? Reason { get; set; }
    }
}
