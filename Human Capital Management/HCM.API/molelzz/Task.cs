using System;
using System.Collections.Generic;

namespace HCM.API.molelzz
{
    public partial class Task
    {
        public int Id { get; set; }
        public string? TaskName { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }
        public string? EmployeeId { get; set; }
        public string? IssuerId { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual Employee? Issuer { get; set; }
        public virtual Priority? Priority { get; set; }
        public virtual Status? Status { get; set; }
    }
}
