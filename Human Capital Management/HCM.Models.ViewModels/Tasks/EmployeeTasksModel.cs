namespace HCM.Models.ViewModels.Tasks
{
    public class EmployeeTasksModel
    {
        public EmployeeTasksModel()
        {
            this.Tasks = new HashSet<TaskModel>();
        }

        public ICollection<TaskModel> Tasks { get; set; }

        public SearchTaskOptionsModel Options { get; set; }

    }
}
