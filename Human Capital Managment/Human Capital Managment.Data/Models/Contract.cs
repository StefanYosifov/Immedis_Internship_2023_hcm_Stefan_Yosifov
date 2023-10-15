using System;
using System.Collections.Generic;

namespace Human_Capital_Managment.Data.Models2
{
    public partial class Contract
    {
        public string Id { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Details { get; set; }
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public string? EmployeeId { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual Salary IdNavigation { get; set; } = null!;
    }
}
