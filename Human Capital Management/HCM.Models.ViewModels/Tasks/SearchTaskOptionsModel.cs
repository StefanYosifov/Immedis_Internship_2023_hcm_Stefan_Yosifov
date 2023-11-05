namespace HCM.Models.ViewModels.Tasks
{
    using Priority;

    using Status;

    public class SearchTaskOptionsModel
    {
        public SearchTaskOptionsModel()
        {
            this.Priorities = new HashSet<PriorityViewModel>();
            this.Statuses = new HashSet<StatusViewModel>();
            this.TaskDays = new List<int>() { 7, 14, 30, 90, 365 };
        }

        public ICollection<PriorityViewModel> Priorities { get; set; }

        public ICollection<StatusViewModel> Statuses { get; set; }

        public ICollection<int> TaskDays { get; set; }

    }
}
