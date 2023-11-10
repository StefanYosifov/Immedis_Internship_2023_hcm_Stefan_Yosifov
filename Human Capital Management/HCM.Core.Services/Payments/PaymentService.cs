namespace HCM.Core.Services.Payments
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Constants;
    using Common.Exceptions_Messages.Employees;
    using Common.Exceptions_Messages.Payments;
    using Common.Helpers;
    using Common.Manager;
    using Countries;
    using Data;
    using Data.Models;
    using Department;
    using Details;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using Models.ViewModels.Payments;
    using Models.ViewModels.Payments.Bonuses;
    using Models.ViewModels.Payments.Enums;
    using Models.ViewModels.Payments.Payroll;
    using Models.ViewModels.Roles;
    using NuGet.Packaging;
    using System.Globalization;

    internal class PaymentService : IPaymentService
    {
        private readonly IMemoryCache cache;
        private readonly ApplicationDbContext context;
        private readonly IStatisticsService statisticsService;
        private readonly IMapper mapper;
        private readonly IDepartmentService departmentService;
        private readonly ICountryService countyService;
        private readonly IEmployeeManager employeeManager;

        public PaymentService(
            ApplicationDbContext context,
            IMapper mapper,
            IStatisticsService statisticsService,
            IMemoryCache cache,
            IDepartmentService departmentService,
            ICountryService countyService,
            IEmployeeManager employeeManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.statisticsService = statisticsService;
            this.cache = cache;
            this.departmentService = departmentService;
            this.countyService = countyService;
            this.employeeManager = employeeManager;
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
                ValidationConstants.PaginationConstants.DefaultItemsPerPage);

            var totalPages = employeesPagination.TotalPages;

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
                await statisticsService.GetAverageSalaryInDepartmentById(departmentId);

            salaryChangeModel.AveragePositionSalary = await statisticsService.GetAverageSalaryInPositionById(departmentId);

            salaryChangeModel.AverageSenioritySalary =
                await statisticsService.GetAverageSalaryInSeniorityById(departmentId);

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
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(CachingConstants.Hours.BonusCachingConstant)
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
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(CachingConstants.Hours.DeductionCachingConstant)
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

        public async Task<PayrollPaginationModel> GetPayrolls(int id, PayRollSearchModel model)
        {
            var allPayrolls = context.Payrolls.AsQueryable();

            if (model.DepartmentId > 0)
            {
                allPayrolls = allPayrolls
                    .Where(p => p.Employee.DepartmentId == model.DepartmentId);
            }

            if (model.StartDate != null)
            {
                var startDate = DateTime.Parse(model.StartDate);
                allPayrolls = allPayrolls
                    .Where(p => p.StartDate >= startDate);
            }

            if (model.EndDate != null)
            {
                var endDate = DateTime.Parse(model.EndDate);
                allPayrolls = allPayrolls
                    .Where(p => p.EndDate <= endDate);
            }

            var mappedPayroll = allPayrolls.ProjectTo<PayrollModel>(mapper.ConfigurationProvider);

            var payroll = await Pagination<PayrollModel>.CreateAsync(mappedPayroll, id,
                ValidationConstants.PaginationConstants.PayrollItemsPerPage);

            foreach (var currentPayroll in payroll)
            {
                currentPayroll.BonusAmount = currentPayroll.Bonuses.Sum(b => b.Amount);
                currentPayroll.DeductionAmount = (decimal)currentPayroll.Deductions.Sum(d => d.Amount);

                var country = await context.Countries.Select(c => new
                {
                    c.Employees,
                    c.TaxRate
                })
                    .FirstOrDefaultAsync(x => x.Employees.Select(e => e.Id).Any(x => x == currentPayroll.EmployeeId));

                if (country == null || country.TaxRate == null)
                {
                    currentPayroll.TaxRate = 20;
                }
                else
                {
                    currentPayroll.TaxRate = (decimal)country.TaxRate!;
                }
            }

            return new PayrollPaginationModel()
            {
                Payroll = payroll,
                Page = model.Page,
                TotalPages = payroll.TotalPages,
                Departments = await departmentService.GetDepartments()
            };
        }

        public async Task<string> CreatePayrollForDepartments(PayrollCreateModel model)
        {
            var employeesInOneOfChosenDepartments = await context.Employees
                .Include(e => e.Salary)
                .Include(e => e.Bonuses)
                .Include(e => e.Deductions)
                .Include(e => e.Payrolls)
                .Select(e => new
                {
                    e.Id,
                    e.DepartmentId,
                    e.Salary,
                    e.Bonuses,
                    e.Deductions,
                    e.Payrolls,
                    e.NationalityId
                })
                .Where(e => e.DepartmentId.HasValue)
                .Where(e => model.DepartmentIds.Any(d => d == e.DepartmentId.Value))
                .ToArrayAsync();


            ICollection<Payroll> payrolls = new List<Payroll>(employeesInOneOfChosenDepartments.Length);

            var result = await context.Payrolls
                .GroupBy(p => new { p.Employee.DepartmentId, p.EndDate })
                .Select(groupedData => new
                {
                    DepartmentId = groupedData.Key.DepartmentId,
                    EndDate = groupedData.Key.EndDate
                }).ToArrayAsync();


            var countriesTaxRates = await countyService.GetCountriesTaxRates();

            var today = DateTime.Today;

            foreach (var employee in employeesInOneOfChosenDepartments)
            {
                DateTime? startDate = null;


                if (string.IsNullOrEmpty(model.StartDate))
                {
                    if (result.Length > 0)
                    {
                        startDate = result.FirstOrDefault(r => r.DepartmentId == employee.DepartmentId).EndDate;
                    }

                    if (startDate == null)
                    {
                        var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
                        startDate = firstDayOfMonth;
                    }
                }
                else
                {
                    startDate = DateTime.Parse(model.StartDate);
                }

                DateTime? endDate = null;

                if (string.IsNullOrEmpty(model.EndDate))
                {
                    endDate = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
                }
                else
                {
                    endDate = DateTime.Parse(model.EndDate);
                }

                ICollection<Bonuse> bonus = new List<Bonuse>();

                if (model.IncludeBonuses)
                {
                    var getBonuses = await context.Bonuses
                        .Where(b => b.PayrollId == null && b.EmployeeId == employee.Id)
                        .ToArrayAsync();

                    if (getBonuses.Length > 0)
                    {
                        bonus.AddRange(getBonuses);
                    }
                }

                ICollection<Deduction> deductions = new List<Deduction>();

                if (model.IncludeDeductions)
                {
                    var getDeductions = await context.Deductions
                          .Where(b => b.PayrollId == null && b.EmployeeId == employee.Id)
                          .ToArrayAsync();

                    if (getDeductions.Length > 0)
                    {
                        deductions.AddRange(getDeductions);
                    }
                }

                var calculateSalary = employee.Salary?.SalaryAmount * (model.Percentage / 100) ?? 0;

                var payroll = new Payroll()
                {
                    Bonuses = employee.Bonuses.Sum(b => b.Amount),
                    StartDate = (DateTime)startDate,
                    EndDate = (DateTime)endDate,
                    Salary = calculateSalary,
                    BonusesNavigation = bonus,
                    DeductionsNavigation = deductions,
                    CreatedOn = DateTime.UtcNow,
                    EmployeeId = employee.Id,
                    Deductions = deductions.Sum(d => d.Amount) ?? 0,
                };

                var taxRate = countriesTaxRates[employee.NationalityId ?? 1];

                if (taxRate is 0 or null)
                {
                    taxRate = 20;
                }

                payroll.GrossPay = payroll.Salary + payroll.Bonuses - payroll.Deductions;
                payroll.NetPay = (decimal)(payroll.GrossPay * (1 - (taxRate / 100)))!;
                payrolls.Add(payroll);
            }

            await context.AddRangeAsync(payrolls);
            await context.SaveChangesAsync();
            return string.Format(PaymentMessages.SuccessfullyAddedPayroll, employeesInOneOfChosenDepartments.Length);
        }

        public async Task<ICollection<PayrollAllUnpaidSalaries>> GetAllUnpaidSalaries()
        {
            var unpaidSalaries = await context.Payrolls
                .Where(p => p.PaidOn == null)
                .GroupBy(p => new
                {
                    p.Employee.DepartmentId,
                    p.Employee.Department!.Name,
                    p.Employee.Department.ImageUrl
                })
                .Select(g => new PayrollAllUnpaidSalaries
                {
                    DepartmentId = (int)g.Key.DepartmentId!,
                    DepartmentName = g.Key.Name,
                    TotalPayment = g.Sum(p => p.Salary),
                    EmployeeCount = g.Count(),
                    DepartmentImageUrl = g.Key.ImageUrl
                })
                .ToArrayAsync();

            return unpaidSalaries;
        }

        public async Task<string> CompletePayrollById(int payrollId)
        {
            if (employeeManager.IsInRole(RolesEnum.Admin) == false || employeeManager.IsInRole(RolesEnum.HR)==false)
            {
                throw new PaymentServiceExceptions(EmployeeMessages.NoAccess);
            }

            var payroll = await context.Payrolls.FindAsync(payrollId);

            if (payroll == null)
            {
                throw new PaymentServiceExceptions(PaymentMessages.InvalidPayroll);
            }

            payroll.PaidOn = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return String.Format(PaymentMessages.SuccessfullyAddedPayroll, payroll.Id);
        }

        public async Task<string> RemovePayrollById(int payrollId)
        {
            var getUserId = employeeManager.GetUserId();

            if (employeeManager.IsInRole(RolesEnum.Admin) == false || employeeManager.IsInRole(RolesEnum.HR) == false)
            {
                throw new PaymentServiceExceptions(EmployeeMessages.NoAccess);
            }

            var payroll = await context.Payrolls.FindAsync(payrollId);

            if (payroll == null)
            {
                throw new PaymentServiceExceptions(PaymentMessages.InvalidPayroll);
            }

            if (payroll.PaidOn != null)
            {
                throw new PaymentServiceExceptions(PaymentMessages.RemovingCompletedPayroll);
            }

             context.Remove(payroll);
             await context.SaveChangesAsync();
             return String.Format(PaymentMessages.SuccessfullyRemovedPayroll,payroll.Id);
        }

        public async Task<ICollection<PayrollEmployeeDetails>> GetPayRollDetailsByEmployeeId(string employeeId)
        {
            return await context.Payrolls
                .ProjectTo<PayrollEmployeeDetails>(mapper.ConfigurationProvider)
                .Where(p=>p.EmployeeId == employeeId && p.IsPaid==true)
                .Take(5)
                .ToArrayAsync();
        }

        private async Task<bool> DoesEmployeeExist(string id)
        {
            var doesEmployeeExist = await context.Employees
                .Select(e => e.Id)
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