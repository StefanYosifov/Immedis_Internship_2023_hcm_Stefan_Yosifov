namespace Human_Capital_Management.Services.Project
{
    using Human_Capital_Managment.ViewModels.ProjectViewModels;

    public interface IProjectService
    {

        Task<ICollection<AllMyProjectsRequestViewModel>> GetAllMyProjects(string userId);

        Task<ICollection<AllMyDepartmentProjectsViewModel>> GetAllMyTeamProjects(string userId);

    }
}
