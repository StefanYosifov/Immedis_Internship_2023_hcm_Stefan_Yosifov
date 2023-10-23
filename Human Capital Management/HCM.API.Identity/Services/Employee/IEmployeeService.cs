namespace HCM.API.Services.Services.Employee
{
    using Common;

    using Models.ViewModels.Countries;
    using Models.ViewModels.Employees;
    using Models.ViewModels.Genders;

    public interface IEmployeeService
    {

        Task<Pagination<EmployeeTableModel>> GetEmployeeTable(int id, EmployeeQueryTableFilters query);

        Task<ICollection<CountryViewModel>> GetCountries();

        Task<ICollection<GenderViewModel>> GetGenders();

        Task<EmployeeCreateModel> GetEmployeeCreationOptions();

        Task<bool> CreateEmployee();
    }
}
