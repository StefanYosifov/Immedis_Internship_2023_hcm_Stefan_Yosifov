namespace HCM.Models.ViewModels.Tasks
{
    public class CreateTaskModel
    {

        public string EmployeeId { get; set; } = null!;

        public string TaskName { get; set; }

        public string Description { get; set; } = null!;

        public DateTime DueDate { get; set; }

        public int PriorityId { get; set; }

        public int StatusId { get; set; }

    }
}
