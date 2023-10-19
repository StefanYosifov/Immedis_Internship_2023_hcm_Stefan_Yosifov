using System;
using System.Collections.Generic;

namespace HCM.Data.Models
{
    public partial class Payroll
    {
        public int PayrollId { get; set; }
        public string? EmployeeId { get; set; }
        public decimal? SalaryAmount { get; set; }
        public decimal? Bonuses { get; set; }
        public decimal? Deductions { get; set; }
        public DateTime? PayrollDate { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}
