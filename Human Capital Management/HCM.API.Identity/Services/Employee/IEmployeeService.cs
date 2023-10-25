namespace HCM.API.Services.Services.Employee
{
    using Common;

    using Models.ViewModels.Countries;
    using Models.ViewModels.Employees;
    using Models.ViewModels.Genders;

    public interface IEmployeeService
    {

        Task<EmployeeTableModel> GetEmployeeTable(int id, EmployeeQueryTableFilters query);

        Task<ICollection<CountryViewModel>> GetCountries();

        Task<ICollection<GenderViewModel>> GetGenders();

        Task<EmployeeCreateRequestModel> GetEmployeeCreationOptions();

        Task<bool> CreateEmployee(EmployeeCreateResponseModel requestModel);

        Task<string> CreateFileFromEmployees(IFormFile file);
    }
}
