namespace HCM.Core.Services.Payments
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Common.Constants;
    using Common.Exceptions_Messages.Employees;
    using Common.Exceptions_Messages.Payments;
    using Common.Helpers;

    using Data;

    using Details;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    using Models.ViewModels.Payments;
    using Models.ViewModels.Payments.Bonuses;
    using Models.ViewModels.Payments.Enums;

    internal class PaymentService : IPaymentService
    {
        private readonly IMemoryCache cache;
        private readonly ApplicationDbContext context;
        private readonly IDetailsService detailsService;
        private readonly IMapper mapper;

        public PaymentService(
            ApplicationDbContext context,
            IMapper mapper,
            IDetailsService detailsService,
            IMemoryCache cache)
        {
            this.context = context;
            this.mapper = mapper;
            this.detailsService = detailsService;
            this.cache = cache;
        }

        public async Task<SalaryTablePagination> GetEmployeeSalaryInformation(int page, SalaryTableQueryModel query)
        {
            var employees = context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(query.Search))
            {
                var wildcard = $"{query.Search.ToLower()}%";

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

            var mappedEmployees = employees.Select(e => new SalaryTableModel
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

            var employeesPagination = await Pagination<SalaryTableModel>.CreateAsync(mappedEmployees, page,
                ValidationConstants.PaginationConstants.ItemsPerPage);

            var totalPages = (int)Math.Ceiling(
                (decimal)employeesPagination.Count / ValidationConstants.PaginationConstants.ItemsPerPage);

            return new SalaryTablePagination
            {
                Employees = employeesPagination,
                TotalPages = totalPages,
                PageNumber = page
            };
        }

        public async Task<SalaryChangeModel> GetEmployeeSalaryInformationById(string id)
        {
            var findEmployee = await context.Employees
                .Include(e => e.Position)
                .Include(e => e.Seniority)
                .Include(e => e.Salary)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (findEmployee == null)
            {
                throw new PaymentServiceExceptions(EmployeeMessages.NotFound);
            }

            var daysInCompany = -1;
            if (findEmployee.CreatedOn != null)
            {
                daysInCompany = DateTime.UtcNow.Subtract((DateTime)findEmployee.CreatedOn!).Days!;
            }

            var salaryChangeModel = mapper.Map<SalaryChangeModel>(findEmployee);

            var departmentId = findEmployee.DepartmentId ?? 0;

            salaryChangeModel.TimeInCompany = daysInCompany;
            salaryChangeModel.AverageDepartmentSalary =
                await detailsService.GetAverageSalaryInDepartmentById(departmentId);
            salaryChangeModel.AveragePositionSalary = await detailsService.GetAverageSalaryInPositionById(departmentId);
            salaryChangeModel.AverageSenioritySalary =
                await detailsService.GetAverageSalaryInSeniorityById(departmentId);

            return salaryChangeModel;
        }

        public async Task<ICollection<BonusReasonModel>> GetAllReasonsForBonuses()
        {
            const string cacheKey = "Bonus_Reasons";

            if (cache.TryGetValue(cacheKey, out ICollection<BonusReasonModel> bonusesReasons))
            {
                return bonusesReasons;
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(CachingConstants.BonusCachingConstant)
            };

            var getBonusesReasons = await context.BonusesReasons
                .AsNoTracking()
                .ProjectTo<BonusReasonModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            cache.Set(cacheKey, getBonusesReasons, cacheEntryOptions);

            return getBonusesReasons;
        }

        public async Task<ICollection<DeductionReasonModel>> GetAllDeductionReasonsForDeduction()
        {
            const string cacheKey = "Deduction_Reasons";

            if (cache.TryGetValue(cacheKey, out ICollection<DeductionReasonModel> bonusesReasons))
            {
                return bonusesReasons;
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(CachingConstants.DeductionCachingConstant)
            };

            var getDeductionReason = await context.DeductionReasons
                .AsNoTracking()
                .ProjectTo<DeductionReasonModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            cache.Set(cacheKey, getDeductionReason, cacheEntryOptions);

            return bonusesReasons;
        }

        public async Task<MonthlyBonusDeductionTableModel> GetMonthlySalaryAdditionsByMonth(
            TableBonusDeductionSearchModel model)
        {
            var doesEmployeeExist = await context.Employees.AnyAsync(e => e.Id == model.EmployeeId);

            if (doesEmployeeExist == false)
            {
                throw new PaymentServiceExceptions(EmployeeMessages.NotFound);
            }

            return new MonthlyBonusDeductionTableModel
            {
                Bonuses = await GetBonusesForTheMonth(model),
                Deductions = await GetDeductionForTheMonth(model)
            };
        }

        private async Task<ICollection<MonthlyBonusTableModel>> GetBonusesForTheMonth(
            TableBonusDeductionSearchModel model)
        {
            return await context.Bonuses
                .Where(b => b.EmployeeId == model.EmployeeId)
                .Select(b => new MonthlyBonusTableModel
                {
                    BonusId = b.Id,
                    BonusReason = new BonusReasonModel
                    {
                        ReasonId = b.ReasonId,
                        ReasonName = b.Reason.Name
                    },
                    DeductionAmount = b.Amount
                }).ToArrayAsync();
        }

        private async Task<ICollection<MonthlyDeductionTableModel>> GetDeductionForTheMonth(
            TableBonusDeductionSearchModel model)
        {
            return await context.Deductions
                .Where(d => d.EmployeeId == model.EmployeeId)
                .Select(b => new MonthlyDeductionTableModel
                {
                    DeductionId = b.Id,
                    DeductionAmount = b.Amount,
                    DeductionReason = new DeductionReasonModel
                    {
                        DeductionId = b.ReasonId,
                        DeductionReason = b.Reason.Name
                    }
                }).ToArrayAsync();
        }
    }
}