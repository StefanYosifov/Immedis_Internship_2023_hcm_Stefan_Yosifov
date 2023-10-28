﻿namespace HCM.Data.Models
{
    using System.Collections.Generic;

    public partial class DeductionReason
    {
        public DeductionReason()
        {
            Deductions = new HashSet<Deduction>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Deduction> Deductions { get; set; }
    }
}