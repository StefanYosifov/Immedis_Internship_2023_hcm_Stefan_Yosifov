namespace HCM.Core.Services.Employee
{
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

        Task<string> CreateEmployeesFromFile(EmployeeFileInternalDTO file);

        Task<EmployeeGetEditModel> GetEmployeeToEdit(string employeeId);

        Task<string> EditEmployee(string employeeId, EmployeeSendEditModel model);

        Task<ICollection<EmployeeSearchModel>> GetEmployeesWithNoDepartmentByName(string? name);

        Task<string> EditEmployeePositionAndSeniority(EmployeeEditPositionAndSeniority model);
    }
}