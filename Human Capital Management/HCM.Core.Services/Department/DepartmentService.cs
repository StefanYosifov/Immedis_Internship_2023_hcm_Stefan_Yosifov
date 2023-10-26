namespace HCM.Core.Services.Department
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Models.ViewModels.Departments;
    using Models.ViewModels.Positions;
    using Models.ViewModels.Seniorities;

    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public DepartmentService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ICollection<DepartmentViewModel>> GetDepartments()
        {
            return await context.Departments
                .ProjectTo<DepartmentViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }
        public async Task<ICollection<PositionViewModel>> GetPositionsByDepartmentId(int id)
        {
            return await context.Positions
                .Where(p => p.DepartmentId == id)
                .ProjectTo<PositionViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public async Task<ICollection<SeniorityViewModel>> GetSenioritiesByPositionId(int id)
        {
            return await context.PositionSeniorities
                .Where(s => s.PositionId == id)
                .Select(s => new SeniorityViewModel()
                {
                    Id = s.SeniorityId,
                    Name = s.Seniority!.Name
                })
                .ToArrayAsync();
        }
    }
}
