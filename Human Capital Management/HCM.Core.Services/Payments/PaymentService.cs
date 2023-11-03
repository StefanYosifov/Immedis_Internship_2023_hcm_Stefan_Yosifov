namespace HCM.Core.Services.Payments
{
    using Common.Constants;
    using Common.Helpers;

    using Data;

    using Models.ViewModels.Payments;
    using Models.ViewModels.Payments.Enums;

    internal class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext context;

        public PaymentService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<SalaryTablePagination> GetEmployeeSalaryInformation(int page, SalaryTableQueryModel query)
        {
            var employees = context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(query.Search))
            {
                var wildcard = $"%{query.Search.ToLower()}%";

                employees = employees
                    .Where(e =>
                        e.Username.ToLower().Contains(wildcard) ||
                        e.FirstName!.ToLower().Contains(wildcard) ||
                        e.LastName!.ToLower().Contains(wildcard));
            }

            if (query.DepartmentId > 0)
            {
                employees = employees.Where(e => e.DepartmentId == query.DepartmentId);
            }

            if (query.PositionId > 0)
            {
                employees = employees.Where(e => e.PositionId == query.PositionId);
            }

            if (query.SeniorityId > 0)
            {
                employees = employees.Where(e => e.SeniorityId == query.SeniorityId);
            }

            employees = query.Sort switch
            {
                SalaryOptionsOrderModel.SalaryAccending => employees.OrderBy(e => e.Salary),
                SalaryOptionsOrderModel.SalaryDeccending => employees.OrderByDescending(e => e.Salary),
                SalaryOptionsOrderModel.EmployeeAgeAccending => employees.OrderBy(e => e.BirthDate),
                SalaryOptionsOrderModel.EmployeeAgeDeccending => employees.OrderByDescending(e => e.BirthDate),
                SalaryOptionsOrderModel.DepartmentAccending => employees.OrderBy(e => e.Department.Name),
                SalaryOptionsOrderModel.DepartmentDeccending => employees.OrderByDescending(e => e.Department.Name),
                SalaryOptionsOrderModel.PositionAccending => employees.OrderBy(e => e.Position.Name),
                SalaryOptionsOrderModel.PositionDeccending => employees.OrderByDescending(e => e.Position.Name),
                SalaryOptionsOrderModel.SeniorityAccending => employees.OrderBy(e => e.Seniority),
                SalaryOptionsOrderModel.SeniorityDeccending => employees.OrderByDescending(e => e.Seniority),
                _ => employees
            };

            var mappedEmployees = employees.Select(e => new SalaryTableModel()
            {
                EmployeeId = e.Id,
                EmployeeFirstname = e.FirstName,
                EmployeeLastname = e.LastName,
                SalaryId = e.Salary.EmployeeId,
                SalaryAmount = e.Salary.SalaryAmount,
                Age = DateCalculator.CalculateAge(e.BirthDate),
                GenderId = e.GenderId,
                DepartmentName = e.Department.Name,
                PositionName = e.Position.Name,
                SeniorityName = e.Seniority.Name
            });

            var employeesPagination = await Pagination<SalaryTableModel>.CreateAsync(mappedEmployees, page, ValidationConstants.PaginationConstants.ItemsPerPage);

            var totalPages = (int)Math.Ceiling(
                (decimal)employeesPagination.Count / ValidationConstants.PaginationConstants.ItemsPerPage);

            return new SalaryTablePagination()
            {
                Employees = employeesPagination,
                TotalPages = totalPages,
                PageNumber = page,
            };
        }
    }
}