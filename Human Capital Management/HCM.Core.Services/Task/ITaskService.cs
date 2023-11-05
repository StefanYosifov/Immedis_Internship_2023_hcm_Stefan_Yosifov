namespace HCM.Core.Services.Task
{
    using Models.ViewModels.Tasks;
    using Models.ViewModels.Tasks.Priority;
    using Models.ViewModels.Tasks.Status;

    public interface ITaskService
    {

        Task<ICollection<PriorityViewModel>> GetTaskPriorities();

        Task<ICollection<StatusViewModel>> GetTaskStatuses();
        Task<SearchTaskOptionsModel> GetTaskOptions();

        Task<EmployeeTasks> GetEmployeeTasks(SearchTaskModel model);

    }
}
