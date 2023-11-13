namespace HCM.Core.Services.Department
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Exceptions_Messages.Departments;
    using Common.Exceptions_Messages.Employees;
    using Common.Helpers;
    using Countries;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using Models.ViewModels.Departments;
    using Models.ViewModels.Departments.Enums;
    using Models.ViewModels.Positions;
    using Models.ViewModels.Seniorities;
    using System.Runtime.InteropServices;

    internal class DepartmentService : IDepartmentService
    {
        private readonly IMemoryCache cache;
        private readonly ApplicationDbContext context;
        private readonly ICountryService countryService;
        private readonly IMapper mapper;

        public DepartmentService(
            ApplicationDbContext context,
            IMapper mapper,
            ICountryService countryService,
            IMemoryCache cache)
        {
            this.context = context;
            this.mapper = mapper;
            this.countryService = countryService;
            this.cache = cache;
        }

        public async Task<ICollection<DepartmentViewModel>> GetDepartments()
        {
            const string cacheKey = "all_departments";

            if (cache.TryGetValue(cacheKey, out ICollection<DepartmentViewModel> departments))
            {
                return departments;
            }

            var getDepartments = await context.Departments
                .AsNoTracking()
                .ProjectTo<DepartmentViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            cache.Set(cacheKey, getDepartments, TimeSpan.FromMinutes(30));

            return getDepartments;
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
                .ProjectTo<SeniorityViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public async Task<ICollection<DepartmentGetAllModel>> GetAllDepartments(DepartmentSendQueryFilters query)
        {
            var departments = context.Departments.AsQueryable();

            if (!string.IsNullOrEmpty(query.Search))
            {
                departments = departments
                    .Where(d => d.Name.ToLower().Contains(query.Search.ToLower()));
            }

            if (query.CountryId > 0)
            {
                departments = departments
                    .Where(d => d.CountryId == query.CountryId);
            }

            departments = query.Sort switch
            {
                DepartmentSortSearch.AvailablePositionsAccending => departments.OrderBy(d =>
                    d.MaxPeopleCount - d.Employees.Count(e => e.DepartmentId == d.Id)),

                DepartmentSortSearch.AvailablePositionsDecending => departments.OrderByDescending(d =>
                    d.MaxPeopleCount - d.Employees.Count(e => e.DepartmentId == d.Id)),

                DepartmentSortSearch.CountryAccending => departments.OrderBy(d => d.Country.Name),

                DepartmentSortSearch.CountryDeccending => departments.OrderByDescending(d => d.Country.Name),

                DepartmentSortSearch.EmployeeCountAccending => departments.OrderBy(d =>
                    d.Employees.Count(e => e.DepartmentId == d.Id)),

                DepartmentSortSearch.EmployeeCountDeccending => departments.OrderByDescending(d =>
                    d.Employees.Count(e => e.DepartmentId == d.Id)),

                DepartmentSortSearch.NameAccending => departments.OrderBy(d => d.Name),

                DepartmentSortSearch.NameDeccending => departments.OrderByDescending(d => d.Name)
            };

            return await departments
                .ProjectTo<DepartmentGetAllModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public async Task<DepartmentGetAllQueryFilters> GetAllQueryFilters()
        {
            return new DepartmentGetAllQueryFilters
            {
                Sort = Enum.GetNames(typeof(DepartmentSortSearch)),
                Countries = await countryService.GetCountries()
            };
        }

        public async Task<DepartmentDetailsViewModel> GetDepartmentDetailsById(int id)
        {
            var getDepartment = await context.Departments
                .Include(d => d.Country)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (getDepartment == null)
            {
                throw new DepartmentServiceExceptions("Such department does not exist");
            }

            var employeesInDepartment = await GetEmployeesInCurrentDepartment(id);

            decimal totalSalary = SumOfAllSalariesInDepartment(employeesInDepartment.Select(ed => ed.Salary));

            if (employeesInDepartment.Count > 0)
            {
                totalSalary /= employeesInDepartment.Count;
            }

            var mappedEmployees = employeesInDepartment
                .Select(ed => new DepartmentEmployeesModel
                {
                    EmployeeId = ed.Id,
                    EmployeeAge = DateCalculator.CalculateAge(ed.BirthDate),
                    EmployeeFirstname = ed.FirstName,
                    EmployeeLastname = ed.LastName,
                    EmployeeGender = ed.Gender?.Name,
                    EmployeePosition = ed.Position?.Name,
                    EmployeeSeniority = ed.Seniority?.Name,
                    EmployeeNationalityISO = ed.Nationality?.Iso
                })
                .ToArray();

            return new DepartmentDetailsViewModel
            {
                DepartmentId = id,
                DepartmentName = getDepartment.Name,
                DepartmentImageUrl = getDepartment.ImageUrl,
                EmployeeCapacity = getDepartment.MaxPeopleCount,
                CountryName = getDepartment.Country.Name,
                AverageSalary = (int)totalSalary!,
                EmployeesCount = employeesInDepartment.Count,
                DepartmentEmployees = mappedEmployees,
                AverageAge = mappedEmployees.Average(e=>e.EmployeeAge),
                AvailablePositionsCollection = await GetAvailablePositionsToAddToDepartmentById(id),
                PositionsInDepartment = await GetPositionsInTheDepartmentById(id),
                Countries = await countryService.GetCountries()
            };
        }

        public async Task<ICollection<DepartmentGetPositionsModel>> GetPositionsInTheDepartmentById(int id)
        {
            return await context.Positions
                .Include(p => p.Employees)
                .Include(p => p.Department)
                .Where(p => p.DepartmentId == id)
                .ProjectTo<DepartmentGetPositionsModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public async Task<ICollection<PositionViewModel>> GetAvailablePositionsToAddToDepartmentById(int id)
        {
            return await context.Positions
                .Where(p => p.DepartmentId != id)
                .ProjectTo<PositionViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public async Task<string> AddPositionToDepartmentById(DepartmentAddPosition model)
        {
            var getDepartment = await context.Departments.FindAsync(model.DepartmentId);

            if (getDepartment == null)
            {
                throw new DepartmentServiceExceptions("Invalid department");
            }

            var getPosition = await context.Positions.FindAsync(model.PositionId);

            if (getPosition == null)
            {
                throw new DepartmentServiceExceptions("Invalid position");
            }

            var getPositionsInDepartment = await GetPositionsByDepartmentId(model.DepartmentId);

            if (getPositionsInDepartment.Any(p => p.Id == model.PositionId))
            {
                throw new InvalidOperationException("Position is already added");
            }

            getDepartment.Positions.Add(getPosition);
            await context.SaveChangesAsync();

            return "Success";
        }

        public async Task<string> RemovePositionFromDepartmentById(DepartmentRemovePosition model)
        {
            var department = await context.Departments
                .Include(d => d.Employees)
                .Include(d => d.Positions)
                .FirstOrDefaultAsync(d => d.Id == model.DepartmentId);

            if (department == null)
            {
                throw new DepartmentServiceExceptions("Invalid Department");
            }

            var position = await context.Positions.FindAsync(model.PositionId);

            if (position == null)
            {
                throw new DepartmentServiceExceptions("Invalid position");
            }

            var findPositionInDepartment = department.Positions
                .FirstOrDefault(p => p.Id == model.PositionId);

            if (findPositionInDepartment == null)
            {
                throw new InvalidOleVariantTypeException("Position not in department");
            }

            department.Positions.Remove(findPositionInDepartment);
            await context.SaveChangesAsync();

            foreach (var employee in department.Employees)
            {
                if (employee.PositionId == model.PositionId)
                {
                    employee.PositionId = null;
                    employee.SeniorityId = null;
                }
            }

            await context.SaveChangesAsync();

            return "Success";
        }

        public async Task<string> AddEmployeeToDepartmentById(DepartmentAddEmployee model)
        {
            var findEmployee = await context.Employees.FindAsync(model.EmployeeId);

            if (findEmployee == null)
            {
                throw new InvalidOperationException("Invalid Employee");
            }

            if (findEmployee.DepartmentId != null)
            {
                throw new InvalidOleVariantTypeException("Employee must not be in Department");
            }

            var findDepartment = await context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == model.DepartmentId);

            if (findDepartment == null)
            {
                throw new InvalidOperationException("Department does not exist");
            }

            if (findDepartment.Employees.Count >= findDepartment.MaxPeopleCount)
            {
                throw new DepartmentServiceExceptions(DepartmentMessages.Department.DepartmentIsFull);
            }

            findEmployee.DepartmentId = model.DepartmentId;
            await context.SaveChangesAsync();

            return string.Format(DepartmentMessages.Department.SuccessfullyAddedAnEmployee,
                $"{findEmployee.FirstName} {findEmployee.LastName}");
        }

        public async Task<string> RemoveEmployeeFromDepartmentById(DepartmentRemoveEmployee model)
        {
            var findEmployee = await context.Employees.FindAsync(model.EmployeeId);

            if (findEmployee == null)
            {
                throw new DepartmentServiceExceptions(EmployeeMessages.NotFound);
            }

            var findDepartment = await context.Departments.FindAsync(model.DepartmentId);

            if (findDepartment == null)
            {
                throw new DepartmentServiceExceptions(DepartmentMessages.Department.NotFound);
            }

            var doesEmployeeExistInDepartment = await context.Departments
                .Include(d => d.Employees)
                .Select(d => new
                {
                    d.Employees,
                    d.Id
                })
                .AnyAsync(d => d.Employees
                    .Any(e => e.Id == model.EmployeeId && e.DepartmentId == model.DepartmentId));

            if (doesEmployeeExistInDepartment == false)
            {
                throw new DepartmentServiceExceptions(DepartmentMessages.Department.EmployeeNotFound);
            }

            findEmployee.DepartmentId = null;
            findEmployee.PositionId = null;
            findEmployee.SeniorityId = null;

            await context.SaveChangesAsync();
            return DepartmentMessages.Success.SuccessfullyRemovedEmployeeFromDepartment;
        }

        public async Task<string> EditDepartmentDetails(DepartmentEditDetails model)
        {
            var findDepartment = await context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == model.DepartmentId);

            if (findDepartment == null)
            {
                throw new DepartmentServiceExceptions(DepartmentMessages.Department.NotFound);
            }

            if (model.MaxPeopleCount < findDepartment.Employees.Count)
            {
                throw new DepartmentServiceExceptions(DepartmentMessages.Department
                    .CannotHaveLessCapacityThanEmployees);
            }

            findDepartment.CountryId = model.CountryId;
            findDepartment.ImageUrl = model.ImageURL;
            findDepartment.MaxPeopleCount = model.MaxPeopleCount;
            findDepartment.Name = model.Name;

            await context.SaveChangesAsync();
            return DepartmentMessages.Success.SuccessfullyEditedDepartmentData;
        }

        public async Task<bool> DoesDepartmentExist(int id)
        {
            return await context.Departments
                .Select(d => d.Id)
                .AnyAsync(x => x == id);
        }

        public async Task<bool> DoesPositionExist(int id)
        {
            return await context.Positions
                .Select(p => p.Id)
                .AnyAsync(x => x == id);
        }

        public async Task<bool> DoesSeniorityExist(int id)
        {
            return await context.Positions
                .Select(s => s.Id)
                .AnyAsync(x => x == id);
        }

        public async Task<bool> DoesDepartmentHaveCapacity(int id)
        {
            var department = await context.Departments
                .Include(d=>d.Employees)
                .Select(d => new
                {
                    d.Id,
                    d.MaxPeopleCount,
                    d.Employees.Count
                })
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department!.Count + 1 > department.MaxPeopleCount)
            {
                return false;
            }

            return true;
        }

        private async Task<ICollection<Employee>> GetEmployeesInCurrentDepartment(int id)
        {
            return await context.Employees
                .Include(e => e.EmployeeRoles)
                .Include(e => e.Payrolls)
                .Include(e => e.Salary)
                .Include(e => e.Gender)
                .Include(e => e.Position)
                .Include(e => e.Seniority)
                .Include(e => e.Nationality)
                .Where(e => e.DepartmentId == id)
                .ToArrayAsync();
        }

        private decimal SumOfAllSalariesInDepartment(IEnumerable<Salary?> salaries)
        {
            decimal averageSalary = 0;

            foreach (var salary in salaries)
            {
                if (salary == null)
                {
                    continue;
                }

                averageSalary += (decimal)salary.SalaryAmount!;
            }
            return averageSalary;
        }
    }
}