namespace HCM.Core.Services.Department
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Countries;

    using Data;
    using Microsoft.EntityFrameworkCore;

    using Models.ViewModels.Departments;
    using Models.ViewModels.Departments.Enums;
    using Models.ViewModels.Positions;
    using Models.ViewModels.Seniorities;

    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly ICountryService countryService;

        public DepartmentService(
            ApplicationDbContext context,
            IMapper mapper,
            ICountryService countryService)
        {
            this.context = context;
            this.mapper = mapper;
            this.countryService = countryService;
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


        public async Task<ICollection<DepartmentGetAllModel>> GetAllDepartments(DepartmentSendQueryFilters query)
        {
            var departments = context.Departments.AsQueryable();

            if (!string.IsNullOrEmpty(query.Search))
            {
                var wildcard = $"%{query.Search.ToLower()}%";

                departments = departments
                    .Where(c => EF.Functions.Like(c.Name.ToLower(), wildcard));
            }

            if (query.CountryId > 0)
            {
                departments = departments.Where(d => d.CountryId == query.CountryId);
            }


            departments = query.Sort switch
            {
                DepartmentSortSearch.AvailablePositionsAccending => departments.OrderBy(d => d.MaxPeopleCount - d.Employees.Count(e => e.DepartmentId == d.Id)),
                DepartmentSortSearch.AvailablePositionsDecending => departments.OrderByDescending(d => d.MaxPeopleCount - d.Employees.Count(e => e.DepartmentId == d.Id)),
                DepartmentSortSearch.CountryAccending => departments.OrderBy(d => d.Country.Name),
                DepartmentSortSearch.CountryDeccending => departments.OrderByDescending(d => d.Country.Name),
                DepartmentSortSearch.EmployeeCountAccending => departments.OrderBy(d => d.Employees.Count(e => e.DepartmentId == d.Id)),
                DepartmentSortSearch.EmployeeCountDeccending => departments.OrderByDescending(d => d.Employees.Count(e => e.DepartmentId == d.Id)),
                DepartmentSortSearch.NameAccending => departments.OrderBy(d => d.Name),
                DepartmentSortSearch.NameDeccending => departments.OrderByDescending(d => d.Name),
            };

            return await departments
                .ProjectTo<DepartmentGetAllModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public async Task<DepartmentGetAllQueryFilters> GetAllQueryFilters()
        {
            return new DepartmentGetAllQueryFilters()
            {
                Sort = (string[])Enum.GetNames(typeof(DepartmentSortSearch)),
                Countries = await countryService.GetCountries()
            };
        }
    }
}
