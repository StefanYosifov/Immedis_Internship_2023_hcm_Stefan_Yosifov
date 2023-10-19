using System;
using System.Collections.Generic;

namespace HCM.Data.Models
{
    public partial class PositionSeniority
    {
        public byte? PositionId { get; set; }
        public byte? SeniorityId { get; set; }

        public virtual Position? Position { get; set; }
        public virtual Seniority? Seniority { get; set; }
    }
}
