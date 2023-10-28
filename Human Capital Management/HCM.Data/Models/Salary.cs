﻿namespace HCM.Data.Models
{
    using System;

    public partial class Salary
    {
        public string EmployeeId { get; set; } = null!;
        public decimal? SalaryAmount { get; set; }
        public DateTime? EffectiveDate { get; set; }

        public virtual Employee Employee { get; set; } = null!;
    }
}
