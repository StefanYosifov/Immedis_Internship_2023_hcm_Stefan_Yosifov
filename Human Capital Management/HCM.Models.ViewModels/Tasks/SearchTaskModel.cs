namespace HCM.Models.ViewModels.Tasks
{
    public class SearchTaskModel
    {

        public string? EmployeeId { get; set; }

        public int PriorityId { get; set; }

        public int StatusId { get; set; }

        public bool HasPassed { get; set; }

    }
}
