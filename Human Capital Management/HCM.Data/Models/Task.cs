namespace HCM.Data.Models
{
    using System;

    public partial class Task
    {
        public int Id { get; set; }
        public string? TaskName { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }
        public string? EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual Priority? Priority { get; set; }
        public virtual Status? Status { get; set; }
    }
}
