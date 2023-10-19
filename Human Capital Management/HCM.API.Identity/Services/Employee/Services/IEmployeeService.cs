namespace HCM.API.Services.Services.Employee.Services
{
    using Common;

    using Data.Models;

    using Models.ViewModels.Countries;
    using Models.ViewModels.Department;
    using Models.ViewModels.Employees;
    using Models.ViewModels.Genders;
    using Models.ViewModels.Positions;
    using Models.ViewModels.Seniorities;

    public interface IEmployeeService
    {

        Task<Pagination<EmployeeTableModel>> GetEmployeeTable(int id,EmployeeQueryTableFilters query);

        Task<ICollection<CountryViewModel>> GetCountries();

        Task<ICollection<GenderViewModel>> GetGenders();

        Task<ICollection<DepartmentViewModel>> GetDepartments();

        Task<ICollection<PositionViewModel>> GetPositionsByDepartmentId(int id);

        Task<ICollection<SeniorityViewModel>> GetSenioritiesByPositionId(int id);

        Task<bool> CreateEmployee();
    }
}
