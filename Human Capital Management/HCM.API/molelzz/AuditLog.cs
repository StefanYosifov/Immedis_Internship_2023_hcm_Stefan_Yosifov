using System;
using System.Collections.Generic;

namespace HCM.API.molelzz
{
    public partial class AuditLog
    {
        public long Id { get; set; }
        public string? EmployeeId { get; set; }
        public string EntityName { get; set; } = null!;
        public string Action { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public string Changes { get; set; } = null!;

        public virtual Employee? Employee { get; set; }
    }
}
