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

        Task<EmployeeTasksModel> GetEmployeeTasks(SearchTaskModel model);

        Task<EmployeeTasksPagination> GetEmployeeTasksInPaginationFormat(SearchEmployeeTasksPagination model);

        Task<ICollection<TaskModel>> GetTasksXDaysFromNowByEmployeeId(SearchTasksByDays model);

        Task<string> CreateTask(CreateTaskModel model);

        Task<string> MarkAsCompleted(int id);

    }
}
