namespace HCM.Core.Services.Details
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Common.Constants;
    using Common.Helpers;
    using Common.Manager;

    using Data;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    using Models.ViewModels.Employees;
    using Models.ViewModels.Statistics;
    using Models.ViewModels.Tasks;

    using Task;

    internal class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext context;
        private readonly IEmployeeManager employeeManager;
        private readonly ITaskService taskService;
        private readonly IMemoryCache cache;
        private readonly IMapper mapper;

        public StatisticsService(
            ApplicationDbContext context,
            IEmployeeManager employeeManager,
            ITaskService taskService,
            IMemoryCache cache,
            IMapper mapper)
        {
            this.context = context;
            this.employeeManager = employeeManager;
            this.taskService = taskService;
            this.cache = cache;
            this.mapper = mapper;
        }

        public async Task<decimal?> GetAverageSalaryInDepartmentById(int id)
        {
            return await context.Employees
                .Select(e => new
                {
                    e.DepartmentId,
                    e.Salary!.SalaryAmount
                })
                .Where(e => e.DepartmentId == id)
                .Select(e => e.SalaryAmount)
                .AverageAsync();
        }

        public async Task<decimal?> GetAverageSalaryInPositionById(int id)
        {
            return await context.Employees
                .Select(e => new
                {
                    e.PositionId,
                    e.Salary!.SalaryAmount
                })
                .Where(e => e.PositionId == id)
                .Select(e => e.SalaryAmount)
                .AverageAsync();
        }

        public async Task<decimal?> GetAverageSalaryInSeniorityById(int id)
        {
            return await context.Employees
                .Select(e => new
                {
                    e.SeniorityId,
                    e.Salary!.SalaryAmount
                })
                .Where(e => e.SeniorityId == id)
                .Select(e => e.SalaryAmount)
                .AverageAsync();
        }

        public async Task<HomePageStatisticsModel> GetHomeStatistics()
        {
            var currentUserId = employeeManager.GetUserId();

            var today = DateTime.UtcNow;

            var employeesThisMonth = await context.Employees
                .Select(e => new
                {
                    e.CreatedOn
                })
                .CountAsync(e => e.CreatedOn!.Value.Year == today.Year && e.CreatedOn.Value.Month == today.Month);

            var bestPerformingEmployees = await BestPerformingEmployees(today);
            var worstPerformingEmployees = await WorstPerformingEmployees(today);
            var busiestEmployees = await BusiestEmployees(today);
            var employeesBirthdays = await EmployeeBirthdays(today);
            var tasksIssuedByMe = await TasksIssuedByMe(currentUserId);
            var upcomingTasks = await taskService.GetTasksXDaysFromNowByEmployeeId(new SearchTasksByDays()
            { DaysFromNow = 14, EmployeeId = currentUserId });

            return new HomePageStatisticsModel
            {
                NumberOfDepartments = await context.Departments.CountAsync(),
                NumberOfEmployees = await context.Employees.CountAsync(),
                NumberOfNewEmployeesThisMonth = employeesThisMonth,
                BestPerformingEmployees = bestPerformingEmployees,
                WorstPerformingEmployees = worstPerformingEmployees,
                UpcomingTasks = upcomingTasks,
                BusiestEmployees = busiestEmployees,
                EmployeeBirthdays = employeesBirthdays,
                TaskIssuedByMe = tasksIssuedByMe
            };
        }

        private async Task<ICollection<EmployeeBirthdayModel>> EmployeeBirthdays(DateTime today)
        {
            const string cacheKey = "statistics_employeeBirthdays";

            if (cache.TryGetValue(cacheKey, out ICollection<EmployeeBirthdayModel> cachedEmployeeBirthdays))
            {
                return cachedEmployeeBirthdays;
            }

            var cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CachingConstants.Minutes.StatisticsBirthdaysCache)
            };

            var employeesBirthdays = await context.Employees
                .Select(e => new
                {
                    e.Id,
                    e.FirstName,
                    e.LastName,
                    e.BirthDate,
                })
                .Where(e => e.BirthDate!.Value.Month == today.Month)
                .Select(e => new EmployeeBirthdayModel()
                {
                    EmployeeId = e.Id,
                    EmployeeName = $"{e.FirstName} {e.LastName}",
                    BirthDate = e.BirthDate.Value.ToShortDateString(),
                    Age = DateCalculator.CalculateAge(e.BirthDate),
                })
                .ToArrayAsync();

            cache.Set(cacheKey, employeesBirthdays, cacheOptions);

            return employeesBirthdays;
        }

        private async Task<ICollection<BusiestEmployeeTaskModel>> BusiestEmployees(DateTime today)
        {
            const string cacheKey = "statistics_busiest";

            if (cache.TryGetValue(cacheKey, out ICollection<BusiestEmployeeTaskModel> cachedBusiestEmployees))
            {
                return cachedBusiestEmployees;
            }

            var cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CachingConstants.Minutes.BusiestEmployees)
            };

            var busiestEmployees = await context.Tasks
                .Select(e => new BusiestEmployeeTaskModel()
                {
                    EmployeeId = e.EmployeeId!,
                    TasksAmount = context.Tasks
                        .Where(t => t.EmployeeId == e.EmployeeId)
                        .Count(t => t.DueDate >= today),
                    EmployeeName = $"{e.Employee!.FirstName} {e.Employee.LastName}"
                })
                .OrderByDescending(be => be.TasksAmount)
                .Take(5)
                .ToArrayAsync();

            cache.Set(cacheKey, busiestEmployees, cacheOptions);

            return busiestEmployees;
        }

        private async Task<ICollection<EmployeeBonusModel>> BestPerformingEmployees(DateTime today)
        {
            const string cacheKey = "statistics_bestPerforming";

            if (cache.TryGetValue(cacheKey, out ICollection<EmployeeBonusModel> cachedEmployeeBestPerforming))
            {
                return cachedEmployeeBestPerforming;
            }

            var cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CachingConstants.Minutes.BestAndWorstPerformingEmployee)
            };

            var employeeBonusModels = await context.Bonuses
                .Select(b => new
                {
                    b.CreatedOn,
                    b.EmployeeId,
                    b.Amount,
                    b.Employee,
                })
                .Where(b => b.CreatedOn.Value.Year == today.Year && b.CreatedOn.Value.Month == today.Month)
                .GroupBy(b => b.EmployeeId)
                .Select(group => new EmployeeBonusModel
                {
                    EmployeeId = group.Key,
                    BonusAmount = group.Sum(b => b.Amount),
                    BonusesCount = group.Count(),
                    EmployeeName = $"{group.First().Employee.FirstName} {group.First().Employee.LastName}"
                })
                .OrderByDescending(b => b.BonusAmount)
                .ThenByDescending(eb => eb.BonusesCount)
                .Take(5)
                .ToArrayAsync();


            cache.Set(cacheKey, employeeBonusModels, cacheOptions);
            return employeeBonusModels;
        }

        private async Task<ICollection<EmployeeDeductionModel>> WorstPerformingEmployees(DateTime today)
        {
            const string cacheKey = "statistics_worstPerforming";

            if (cache.TryGetValue(cacheKey, out ICollection<EmployeeDeductionModel> cachedEmployeeWorstPerforming))
            {
                return cachedEmployeeWorstPerforming;
            }

            var cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CachingConstants.Minutes.BestAndWorstPerformingEmployee)
            };

            var worstPerformingEmployees = await context.Deductions
                .Select(d => new
                {
                    d.CreatedOn,
                    d.EmployeeId,
                    d.Amount,
                    d.Employee
                })
                .Where(d => d.CreatedOn.Value.Year == today.Year && d.CreatedOn.Value.Month == today.Month)
                .GroupBy(d => d.EmployeeId)
                .Select(group => new EmployeeDeductionModel
                {
                    EmployeeId = group.Key,
                    DeductionAmount = group.Sum(d => d.Amount),
                    DeductionCount = group.Count(),
                    EmployeeName = $"{group.First().Employee!.FirstName} {group.First().Employee.LastName}"
                })
                .OrderByDescending(d => d.DeductionAmount)
                .ThenByDescending(d => d.DeductionCount)
                .Take(5)
                .ToArrayAsync();

            cache.Set(cacheKey, worstPerformingEmployees, cacheOptions);
            return worstPerformingEmployees;
        }

        private async Task<ICollection<TaskModel>> TasksIssuedByMe(string userId)
        {
            return await context.Tasks
                .Include(t => t.Employee)
                .ProjectTo<TaskModel>(mapper.ConfigurationProvider)
                .Where(t => t.IssuerId == userId && t.Status.StatusName != "Completed")
                .ToArrayAsync();
        }
    }
}