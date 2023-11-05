namespace HCM.Core.Services.Payments
{
    using System.Globalization;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Common.Constants;
    using Common.Exceptions_Messages.Employees;
    using Common.Exceptions_Messages.Payments;
    using Common.Helpers;

    using Data;
    using Data.Models;

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

        public async Task<MonthlyBonusDeductionTableModel> GetMonthlyAdditionTables(
            TableBonusDeductionSearchModel model)
        {
            var doesEmployeeExist = await DoesEmployeeExist(model.EmployeeId);

            DateTime? date = DateTime.ParseExact(model.MonthYearOfSearch, "dd-MM-yyyy", CultureInfo.InvariantCulture);


            if (date == null)
            {
                date = DateTime.UtcNow;
            }

            if (doesEmployeeExist == false)
            {
                throw new PaymentServiceExceptions(EmployeeMessages.NotFound);
            }

            return new MonthlyBonusDeductionTableModel
            {
                Bonuses = await GetBonusesForTheMonth(model, date),
                Deductions = await GetDeductionForTheMonth(model, date)
            };
        }

        public async Task<string> ChangeEmployeeSalary(SalaryChangeRequestModel model)
        {
            var doesEmployeeExist = await DoesEmployeeExist(model.EmployeeId);

            if (doesEmployeeExist == false)
            {
                throw new PaymentServiceExceptions(EmployeeMessages.NotFound);
            }

            var findSalary = await context.Salaries.FindAsync(model.EmployeeId);

            if (findSalary == null)
            {
                var salary = new Salary
                {
                    EmployeeId = model.EmployeeId,
                    EffectiveDate = DateTime.UtcNow,
                    SalaryAmount = model.SalaryAmount
                };

                await context.Salaries.AddAsync(salary);
                await context.SaveChangesAsync();
                return PaymentMessages.SuccessfullyChangedSalary;
            }

            context.Entry(findSalary).CurrentValues.SetValues(model);
            findSalary.EffectiveDate = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return PaymentMessages.SuccessfullyChangedSalary;
        }

        public async Task<string> AddBonus(BonusAddModel model)
        {
            var doesEmployeeExist = await DoesEmployeeExist(model.EmployeeId);

            if (doesEmployeeExist == false)
            {
                throw new PaymentServiceExceptions(EmployeeMessages.NotFound);
            }

            if (model.Amount < 1)
            {
                throw new PaymentServiceExceptions(PaymentMessages.BonusOrDeductionCannotBeBelowOne);
            }

            var doesReasonExists = await context.BonusesReasons
                .Select(br => br.Id)
                .AnyAsync(x => x == model.ReasonId);

            if (doesReasonExists == false)
            {
                throw new PaymentServiceExceptions(PaymentMessages.ReasonNotFound);
            }

            Bonuse bonus = new Bonuse()
            {
                Amount = model.Amount,
                ReasonId = model.ReasonId,
                EmployeeId = model.EmployeeId
            };

            await context.Bonuses.AddAsync(bonus);
            await context.SaveChangesAsync();

            return PaymentMessages.SuccessfullyAddedBonus;
        }

        public async Task<string> AddDeduction(DeductionAddModel model)
        {
            var doesEmployeeExist = await DoesEmployeeExist(model.EmployeeId);

            if (doesEmployeeExist == false)
            {
                throw new PaymentServiceExceptions(EmployeeMessages.NotFound);
            }

            if (model.Amount < 1)
            {
                throw new PaymentServiceExceptions(PaymentMessages.BonusOrDeductionCannotBeBelowOne);
            }

            var doesReasonExists = await context.DeductionReasons
                .Select(br => br.Id)
                .AnyAsync(x => x == model.ReasonId);

            if (doesReasonExists == false)
            {
                throw new PaymentServiceExceptions(PaymentMessages.ReasonNotFound);
            }

            var deduction = new Deduction()
            {
                EmployeeId = model.EmployeeId,
                Amount = model.Amount,
                ReasonId = model.ReasonId,
            };

            await context.Deductions.AddAsync(deduction);
            await context.SaveChangesAsync();

            return PaymentMessages.SuccessfullyAddedDeduction;
        }

        private async Task<bool> DoesEmployeeExist(string id)
        {
            var doesEmployeeExist = await context.Employees
                .Select(e=>e.Id)
                .AnyAsync(x => x == id);

            return doesEmployeeExist;
        }

        private async Task<ICollection<MonthlyBonusTableModel>> GetBonusesForTheMonth(
            TableBonusDeductionSearchModel model, DateTime? date)
        {
            var bonuses = await context.Bonuses.ToArrayAsync();

            return await context.Bonuses
                .Where(b => b.EmployeeId == model.EmployeeId &&
                            b.CreatedOn.Value.Year == date!.Value.Year
                            && b.CreatedOn.Value.Month == date.Value.Month)
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
            TableBonusDeductionSearchModel model, DateTime? date)
        {
            return await context.Deductions
                .Where(d => d.EmployeeId == model.EmployeeId &&
                            d.CreatedOn.Value.Year == date!.Value.Year
                            && d.CreatedOn.Value.Month == date.Value.Month)
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