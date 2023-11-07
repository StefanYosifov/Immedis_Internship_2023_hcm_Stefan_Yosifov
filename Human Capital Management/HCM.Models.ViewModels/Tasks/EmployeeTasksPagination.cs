namespace HCM.Models.ViewModels.Tasks
{
    public class EmployeeTasksPagination
    {
        public EmployeeTasksPagination()
        {
            this.Tasks = new HashSet<TaskModel>();
        }

        public ICollection<TaskModel> Tasks { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}
