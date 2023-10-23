namespace HCM.API.Services.Services.Employee
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Common;
    using Data.Models;

    using Microsoft.EntityFrameworkCore;

    using Models.ViewModels.Countries;
    using Models.ViewModels.Departments;
    using Models.ViewModels.Employees;
    using Models.ViewModels.Employees.Enum;
    using Models.ViewModels.Genders;
    using Models.ViewModels.Positions;
    using Models.ViewModels.Seniorities;

    using ApplicationDbContext = Data.ApplicationDbContext;

    public class EmployeeService : IEmployeeService
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public EmployeeService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Pagination<EmployeeTableModel>> GetEmployeeTable(int id, EmployeeQueryTableFilters query)
        {
            IQueryable<Employee> employeeTable = context.Employees
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.SearchEmployeeName))
            {
                var wildcard = $"%{query.SearchEmployeeName.ToLower()}%";

                employeeTable = employeeTable
                    .Where(c => EF.Functions.Like(c.Username.ToLower(), wildcard));
            }

            if (query.DepartmentId != null)
            {
                employeeTable = employeeTable.Where(e => e.DepartmentId == query.DepartmentId);
            }

            if (query.PositionId != null)
            {
                employeeTable = employeeTable.Where(e => e.PositionId == query.PositionId);
            }

            employeeTable = query.Sort switch
            {
                EmployeeTableSortEnum.FirstNameAccending => employeeTable.OrderBy(e => e.FirstName),
                EmployeeTableSortEnum.FirstNameDeccending => employeeTable.OrderByDescending(e => e.FirstName),
                EmployeeTableSortEnum.LastNameAccending => employeeTable.OrderBy(e => e.LastName),
                EmployeeTableSortEnum.LastNameDeccending => employeeTable.OrderByDescending(e => e.LastName),
                EmployeeTableSortEnum.UsernameAccending => employeeTable.OrderBy(e => e.Username),
                EmployeeTableSortEnum.UsernameDeccending => employeeTable.OrderByDescending(e => e.Username),
                EmployeeTableSortEnum.Youngest => employeeTable.OrderBy(e => e.BirthDate),
                EmployeeTableSortEnum.Oldest => employeeTable.OrderByDescending(e => e.BirthDate),
                EmployeeTableSortEnum.DepartmentNameAccending => employeeTable.OrderBy(e => e.Department.Name),
                EmployeeTableSortEnum.DepartmentDeccending => employeeTable.OrderByDescending(e => e.Department.Name),
                EmployeeTableSortEnum.PositionNameAccending => employeeTable.OrderBy(e => e.Position.Name),
                EmployeeTableSortEnum.PositionDeccending => employeeTable.OrderByDescending(e => e.Position.Name),
                _ => employeeTable
            };


            var mappedTable = employeeTable
                .ProjectTo<EmployeeTableModel>(mapper.ConfigurationProvider);

            return await Pagination<EmployeeTableModel>.CreateAsync(mappedTable);


        }

        public async Task<ICollection<CountryViewModel>> GetCountries()
        {
            return await context.Countries
                .ProjectTo<CountryViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public async Task<ICollection<GenderViewModel>> GetGenders()
        {
            return await context.Genders
                .ProjectTo<GenderViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public Task<EmployeeCreateModel> GetEmployeeCreationOptions()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateEmployee()
        {
            throw new NotImplementedException();
        }
    }
}
