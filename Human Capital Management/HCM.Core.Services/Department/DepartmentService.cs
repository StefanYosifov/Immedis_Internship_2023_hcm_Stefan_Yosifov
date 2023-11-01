﻿namespace HCM.Core.Services.Department
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Helpers;
    using Countries;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models.ViewModels.Departments;
    using Models.ViewModels.Departments.Enums;
    using Models.ViewModels.Positions;
    using Models.ViewModels.Seniorities;
    using System.Runtime.InteropServices;

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

        public async Task<DepartmentDetailsViewModel> GetDepartmentDetailsById(int id)
        {
            var getDepartment = await context.Departments
                .Include(d => d.Country)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (getDepartment == null)
            {
                throw new InvalidOperationException("Such department does not exist");
            }

            var employeesInDepartment = await GetEmployeesInCurrentDepartment(id);
            decimal? averageSalary = 0;

            foreach (var salary in employeesInDepartment.Select(ed => ed.Salary))
            {
                if (salary == null)
                {
                    continue;
                }
                averageSalary += salary.SalaryAmount;
            }

            if (employeesInDepartment.Count > 0)
            {
                averageSalary /= employeesInDepartment.Count;
            }

            var datTimeToday = DateTime.UtcNow;

            var mappedEmployees = employeesInDepartment.Select(ed => new DepartmentEmployeesModel()
            {
                EmployeeId = ed.Id,
                EmployeeAge = DateCalculator.CalculateAge(ed.BirthDate),
                EmployeeFirstname = ed.FirstName,
                EmployeeLastname = ed.LastName,
                EmployeeGender = ed.Gender?.Name,
                EmployeePosition = ed.Position?.Name,
                EmployeeSeniority = ed.Seniority?.Name,
                EmployeeNationalityISO = ed.Nationality?.Iso
            }).ToArray();

            return new DepartmentDetailsViewModel()
            {
                DepartmentId = id,
                DepartmentName = getDepartment.Name,
                DepartmentImageUrl = getDepartment.ImageUrl,
                EmployeeCapacity = getDepartment.MaxPeopleCount,
                CountryName = getDepartment.Country.Name,
                AverageSalary = (int)averageSalary,
                EmployeesCount = employeesInDepartment.Count,
                DepartmentEmployees = mappedEmployees,
                AvailablePositionsCollection = await GetAvailablePositionsToAddToDepartmentById(id),
                PositionsInDepartment = await GetPositionsInTheDepartmentById(id)
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
            var department = await context.Departments.FindAsync(model.DepartmentId);

            if (department == null)
            {
                throw new InvalidOperationException("Invalid department");
            }


            var getPosition = await context.Positions.FindAsync(model.PositionId);

            if (getPosition == null)
            {
                throw new InvalidOperationException("Invalid position");
            }

            var getPositionsInDepartment = await GetPositionsByDepartmentId(model.DepartmentId);

            if (getPositionsInDepartment.Any(p => p.Id == model.PositionId))
            {
                throw new InvalidOperationException("Position is already added");
            }

            department.Positions.Add(getPosition);
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
                throw new InvalidOleVariantTypeException("Invalid Department");
            }

            var position = await context.Positions.FindAsync(model.PositionId);

            if (position == null)
            {
                throw new InvalidOperationException("Invalid position");
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

            var doesDepartmentExist = await context.Departments
                .AnyAsync(d => d.Id == model.DepartmentId);

            if (doesDepartmentExist == false)
            {
                throw new InvalidOperationException("Department does not exist");
            }

            findEmployee.DepartmentId = model.DepartmentId;
            await context.SaveChangesAsync();

            return "You have successfully added the employee to the department";
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
    }
}
