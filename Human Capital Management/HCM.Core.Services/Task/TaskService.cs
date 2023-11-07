namespace HCM.Core.Services.Task
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Common.Constants;
    using Common.Exceptions_Messages.Employees;
    using Common.Exceptions_Messages.Tasks;
    using Common.Helpers;
    using Common.Manager;

    using Data;
    using Data.Models;

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
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(CachingConstants.Hours.DeductionCachingConstant)
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
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(CachingConstants.Hours.DeductionCachingConstant)
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

        public async Task<EmployeeTasksModel> GetEmployeeTasks(SearchTaskModel model)
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


            var employeeTasks = tasks.Select(t => new TaskModel()
            {
                Id = t.Id,
                EmployeeId = t.EmployeeId,
                TaskName = t.TaskName,
                Description = t.Description,
                IssuerId = t.IssuerId,
                DueDate = t.DueDate,
                Priority = new PriorityViewModel()
                {
                    Id = t.PriorityId,
                    PriorityName = t.Priority!.PriorityName
                },
                Status = new StatusViewModel()
                {
                    Id = t.StatusId,
                    StatusName = t.Status!.StatusName
                }
            }).ToArrayAsync();

            return new EmployeeTasksModel()
            {
                Tasks = await employeeTasks,
                Options = await GetTaskOptions(),   
            };


        }

        public async Task<EmployeeTasksPagination> GetEmployeeTasksInPaginationFormat(SearchEmployeeTasksPagination model)
        {
                if(string.IsNullOrEmpty(model.EmployeeId))
                {
                    model.EmployeeId = employeeManager.GetUserId();
                }

            var tasks = context.Tasks
                .Include(t=>t.Status)
                .Include(t=>t.Priority)
                .Where(t => t.EmployeeId == model.EmployeeId)
                .ProjectTo<TaskModel>(mapper.ConfigurationProvider)
                .AsQueryable();

            var paginatedTasks = await Pagination<TaskModel>.CreateAsync(tasks, model.Page,ValidationConstants.PaginationConstants.TasksItemPerPage);

            var totalPages = (int)Math.Ceiling((double)(await tasks.CountAsync()) / ValidationConstants.PaginationConstants.TasksItemPerPage);


            return new EmployeeTasksPagination()
            {
                Tasks = paginatedTasks,
                TotalPages = totalPages,
                Page = model.Page,
            };
        }

        public async Task<ICollection<TaskModel>> GetTasksXDaysFromNowByEmployeeId(SearchTasksByDays model)
        {
            var searchUntil = DateTime.UtcNow.AddDays(model.DaysFromNow);

            return await context.Tasks
                .Include(t=>t.Status)
                .Include(t=>t.Priority)
                .ProjectTo<TaskModel>(mapper.ConfigurationProvider)
                .Where(t => t.DueDate >= searchUntil && t.EmployeeId == model.EmployeeId)
                .ToArrayAsync();
        }

        public async Task<string> CreateTask(CreateTaskModel model)
        {
            var findEmployee = await context.Employees.FindAsync(model.EmployeeId);

            if (findEmployee == null)
            {
                throw new TaskServiceExceptions(EmployeeMessages.NotFound);
            }

            var task = mapper.Map<Task>(findEmployee);
            task.IssuerId = employeeManager.GetUserId();

            await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync();

            return TaskMessages.SuccessfullyCreatedATask;
        }
    }
}
