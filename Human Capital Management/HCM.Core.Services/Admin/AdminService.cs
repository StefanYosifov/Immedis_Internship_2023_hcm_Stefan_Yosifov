namespace HCM.Core.Services.Admin
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Constants;
    using Common.Exceptions_Messages.Admin;
    using Common.Helpers;
    using Countries;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models.ViewModels.Admin;
    using Models.ViewModels.Roles;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Data.Models;

    internal class AdminService : IAdminService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly ICountryService countryService;

        public AdminService(
            ApplicationDbContext context,
            IMapper mapper,
            ICountryService countryService)
        {
            this.context = context;
            this.mapper = mapper;
            this.countryService = countryService;
        }

        public async Task<AuditLogsPagination> GetAuditLogs(int page)
        {
            var auditLogs = context.AuditLogs
                .AsQueryable()
                .ProjectTo<AuditLogs>(mapper.ConfigurationProvider)
                .OrderByDescending(a => a.Timestamp);

            var paginatedAuditLogs = await Pagination<AuditLogs>.CreateAsync(auditLogs, page,
                ValidationConstants.PaginationConstants.AuditItemsPerPage);

            var totalPages = (int)Math.Ceiling((decimal)auditLogs.Count() / ValidationConstants.PaginationConstants.AuditItemsPerPage);

            return new AuditLogsPagination()
            {
                TotalPages = totalPages,
                Page = page,
                AuditLogs = paginatedAuditLogs,
            };

        }

        public async Task<AdminEmployeePagination> GetEmployees(AdminSearchEmployee model)
        {
            var employees = context.Employees.AsQueryable();

            if (model.IsHR == true)
            {
                employees = employees
                    .Where(e => e.EmployeeRoles
                        .Any(er => er.Role.Name == RolesEnum.HR.ToString() && er.Role.Name != RolesEnum.Admin.ToString()));
            }

            var mappedEmployees = employees.Select(e => new AdminEmployeeModel()
            {
                EmployeeId = e.Id,
                EmployeeSeniority = e.Seniority!.Name,
                EmployeeDepartmentName = e.Department!.Name,
                RoleId = e.EmployeeRoles.FirstOrDefault(r => r.Role.Name != RolesEnum.Admin.ToString())!.RoleId,
                EmployeeFirstName = e.FirstName!,
                EmployeeLastName = e.LastName!,
                EmployeePositionName = e.Position!.Name,
                EmployeeUserName = e.Username,
                EmployeeAge = DateCalculator.CalculateAge(e.BirthDate),
            });

            var totalPages = (int)Math.Ceiling((decimal)mappedEmployees.Count() / ValidationConstants.PaginationConstants.AuditItemsPerPage);


            var paginationOfMappedEmployees = await Pagination<AdminEmployeeModel>.CreateAsync(mappedEmployees,
                model.Page, ValidationConstants.PaginationConstants.AdminEmployeesPerPage);

            return new AdminEmployeePagination()
            {
                Employees = paginationOfMappedEmployees,
                TotalPages = totalPages,
                Page = model.Page,
                Roles = await GetAllRoles()
            };
        }

        public async Task<ICollection<RoleViewModel>> GetAllRoles()
        {
            return await context.Roles
                .ProjectTo<RoleViewModel>(mapper.ConfigurationProvider)
                .Where(r => r.Name != RolesEnum.Admin.ToString())
                .ToArrayAsync();
        }

        public Task<string> ChangeEmployeeRole(AdminChangeRole model)
        {
            if (model.RoleId > 0)
            {

            }

            throw new NotImplementedException();
        }

        public async Task<AdminDepartmentsCollection> GetDepartments()
        {
            var countries = await countryService.GetCountries();

            var departments = await context.Departments
                .Include(d => d.Country)
                .Select(d => new AdminDepartmentsModel()
                {
                    DepartmentId = d.Id,
                    DepartmentName = d.Name,
                    DepartmentImageUrl = d.ImageUrl,
                    EmployeeCapacity = d.MaxPeopleCount,
                    CountryName = d.Country!.Name,
                }).ToListAsync();

            return new AdminDepartmentsCollection()
            {
                Countries = countries,
                Departments = (ICollection<AdminDepartmentsModel>)departments
            };

        }

        async Task<string> IAdminService.CreateDepartment(AdminCreateDepartment model)
        {
            var doesCountryExist = await countryService.DoesCountryExist(model.CountryId);

            if (doesCountryExist == false)
            {
                throw new AdminServiceExceptions(AdminMessages.InvalidCountry);
            }

            var doesDepartmentWithTheSameNameExist = await context.Departments.Select(d => new
            {
                d.Name
            }).AnyAsync(d => d.Name == model.Name);

            if (doesDepartmentWithTheSameNameExist == true)
            {
                throw new AdminServiceExceptions(AdminMessages.DepartmentAlreadyExists);
            }

            var department = mapper.Map<Department>(model);
            await context.Departments.AddAsync(department);
            await context.SaveChangesAsync();

            return AdminMessages.SuccessfullyCreatedDepartment;
        }

    }
}
