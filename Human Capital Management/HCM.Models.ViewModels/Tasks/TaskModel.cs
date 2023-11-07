namespace HCM.Models.ViewModels.Tasks
{
    using System;

    using Priority;

    using Status;

    public class TaskModel
    {
        public int Id { get; set; }

        public string TaskName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string EmployeeId { get; set; } = null!;

        public string EmployeeName { get; set; } = null!;
        public string IssuerId { get; set; } = null!;

        public string IssuerName { get; set; } = null!;

        public DateTime? DueDate { get; set; }

        public StatusViewModel Status { get; set; } = null!;

        public PriorityViewModel Priority { get; set; } = null!;
    }
}
