namespace HCM.Core.Services.Admin
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Constants;
    using Common.Helpers;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models.ViewModels.Admin;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    internal class AdminService : IAdminService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AdminService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
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
                    .Where(e => e.EmployeeRoles.Any(er => er.Role.Name == "HR"));
            }

            var mappedEmployees = employees.Select(e => new AdminEmployeeModel()
            {
                EmployeeId = e.Id,
                EmployeeSeniority = e.Seniority!.Name,
                EmployeeDepartmentName = e.Department!.Name,
                RoleId = e.EmployeeRoles.FirstOrDefault(r => r.Role.Name != "Admin")!.RoleId,
                EmployeeFirstName = e.FirstName!,
                EmployeeLastName = e.LastName!,
                EmployeePositionName = e.Position!.Name,
                EmployeeUserName = e.Username,
                EmployeeAge = DateCalculator.CalculateAge(e.BirthDate),
            });

            var totalPages = (int)Math.Ceiling((decimal)mappedEmployees.Count() / ValidationConstants.PaginationConstants.AuditItemsPerPage);


            var paginationOfMappedEmployees = await Pagination<AdminEmployeeModel>.CreateAsync(mappedEmployees,
                model.Page, ValidationConstants.PaginationConstants.AdminEmployeesPerPage);

            return  new AdminEmployeePagination()
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
                .ToArrayAsync();
        }
    }
}
