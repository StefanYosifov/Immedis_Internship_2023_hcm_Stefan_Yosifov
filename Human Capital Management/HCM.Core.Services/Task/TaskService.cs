namespace HCM.Core.Services.Task
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Common.Constants;
    using Common.Manager;

    using Data;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    using Models.ViewModels.Tasks;
    using Models.ViewModels.Tasks.Priority;
    using Models.ViewModels.Tasks.Status;

    internal class TaskService : ITaskService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;
        private readonly IEmployeeManager employeeManager;

        public TaskService(
            ApplicationDbContext context,
            IMapper mapper,
            IMemoryCache cache,
            IEmployeeManager employeeManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.cache = cache;
            this.employeeManager = employeeManager;
        }

        public async Task<ICollection<PriorityViewModel>> GetTaskPriorities()
        {
            const string cacheKey = "task_priorities";

            if (cache.TryGetValue(cacheKey, out ICollection<PriorityViewModel> cachedPriorityCollection))
            {
                return cachedPriorityCollection;
            }

            var getTaskPriorities = await context.Priorities
                .ProjectTo<PriorityViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();


            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(CachingConstants.DeductionCachingConstant)
            };

            cache.Set(cacheKey, getTaskPriorities, cacheEntryOptions);
            return getTaskPriorities;
        }

        public async Task<ICollection<StatusViewModel>> GetTaskStatuses()
        {
            const string cacheKey = "task_statuses";

            if (cache.TryGetValue(cacheKey, out ICollection<StatusViewModel> cachedTaskStatuses))
            {
                return cachedTaskStatuses;
            }

            var getTaskStatuses = await context.Statuses
                .ProjectTo<StatusViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();


            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(CachingConstants.DeductionCachingConstant)
            };

            cache.Set(cacheKey, getTaskStatuses, cacheEntryOptions);
            return getTaskStatuses;
        }

        public async Task<SearchTaskOptionsModel> GetTaskOptions()
        {
            return new SearchTaskOptionsModel()
            {
                Statuses = await GetTaskStatuses(),
                Priorities = await GetTaskPriorities(),
            };
        }

        public async Task<ICollection<EmployeeTasks>> GetEmployeeTasks(SearchTaskModel model)
        {
            var tasks = context.Tasks.AsQueryable();

            if (string.IsNullOrEmpty(model.EmployeeId))
            {
                var currentUserId = employeeManager.GetUserId();
                tasks = tasks.Where(e => e.EmployeeId == currentUserId);
            }

            if (model.PriorityId > 0)
            {
                tasks = tasks.Where(t => t.PriorityId == model.PriorityId);
            }

            if (model.StatusId > 0)
            {
                tasks = tasks.Where(t => t.PriorityId == model.PriorityId);
            }

            if (model.HasPassed)
            {
                tasks = tasks.Where(t => t.DueDate >= DateTime.UtcNow);
            }

            tasks = await tasks.Select(t => new EmployeeTasks()
            {
                Id = t.Id,
                EmployeeId = t.EmployeeId,
                TaskName = t.TaskName,
                Description = t.Description,
                IssuerId = t.IssuerId,
                DueDate = t.DueDate,

            }).ToArrayAsync();
        }
    }
}
