namespace Human_Capital_Management.Services.Project
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Human_Capital_Managment.Data;
    using Human_Capital_Managment.ViewModels.ProjectViewModels;

    using Microsoft.EntityFrameworkCore;

    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ProjectService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ICollection<AllMyProjectsRequestViewModel>> GetAllMyProjects(string userId)
        {
            return await context.Projects
                .Where(p => p.Employees.Any(e => e.Id == userId))
                .ProjectTo<AllMyProjectsRequestViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public async Task<ICollection<AllMyDepartmentProjectsViewModel>> GetAllMyTeamProjects(string userId)
        {
            var getEmployeeDepartment = await context.Departments
                .Select(d => new
                {
                    d.Id,
                    d.DepartmentName,
                    d.Employees
                })
                .Where(d => d.Employees.Any(e => e.Id == userId))
                .Select(d => new
                {
                    d.Id,
                    d.DepartmentName
                })
                .FirstOrDefaultAsync();

            var projects = await context.Projects
                .Where(p =>
                    p.Employees.Any(e => e.DepartmentId == getEmployeeDepartment.Id))
                .ProjectTo<AllMyDepartmentProjectsViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            foreach (var project in projects)
            {
                project.DepartmentId = getEmployeeDepartment.Id;
                project.DepartmentName = getEmployeeDepartment.DepartmentName;
            }

            return projects;
        }
    }
}