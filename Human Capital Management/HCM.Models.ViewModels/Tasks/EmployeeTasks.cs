namespace HCM.Models.ViewModels.Tasks
{
    using Priority;

    using Status;

    public class EmployeeTasks
    {
        public EmployeeTasks()
        {
            this.Statuses = new HashSet<StatusViewModel>();
            this.Priorities = new HashSet<PriorityViewModel>();
        }

        public int Id { get; set; }

        public string TaskName { get; set; }

        public string Description { get; set; }

        public string EmployeeId { get; set; }

        public string IssuerId { get; set; }

        public DateTime? DueDate { get; set; }

        public StatusViewModel Status { get; set; }

        public PriorityViewModel Priority { get; set; }

        //Todo everything above to another class

        public ICollection<StatusViewModel> Statuses { get; set; }

        public ICollection<PriorityViewModel> Priorities { get; set; }


    }
}
