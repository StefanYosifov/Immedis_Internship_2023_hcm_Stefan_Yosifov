namespace Huamn_Capital_Management_API.Services.Employee
{
    using Human_Capital_Managment.Data;
    using Human_Capital_Managment.ViewModels.CountryViewModels;
    using Human_Capital_Managment.ViewModels.Department;
    using Human_Capital_Managment.ViewModels.Employee;
    using Human_Capital_Managment.ViewModels.Gender;
    using Human_Capital_Managment.ViewModels.Status;

    using Microsoft.EntityFrameworkCore;

    public class EmployeeService:IEmployeeService
    {
        private readonly ApplicationDbContext context;

        public EmployeeService(
            ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<SearchFiltersRequestModel> GetSearchInformation()
        {
            return new SearchFiltersRequestModel()
            {
                Countries = await context.Countries.Select(c=>new CountryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ISO = c.Iso
                }).
                    ToArrayAsync(),
                Departments = await context.Departments.Select(d=>new DepartmentViewModel
                {
                    Id = d.Id,
                    Name = d.DepartmentName
                }).
                    ToArrayAsync(),
                Genders = await context.Genders.Select(g=>new GenderViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                })
                    .ToArrayAsync(),
                Statuses = await context.EmployeeStatuses.Select(es=>new StatusViewModel
                {
                    Id = es.Id,
                    Name = es.Name,
                }).ToArrayAsync()
            };
        }

        public Task<ICollection<EmployeeViewModel>> GetEmployees(SearchFiltersRequestModel searchFilters)
        {
            throw new NotImplementedException();
        }
    }
}
