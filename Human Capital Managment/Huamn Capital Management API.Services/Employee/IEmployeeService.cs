namespace Huamn_Capital_Management_API.Services.Employee
{
    using Human_Capital_Managment.ViewModels.Employee;

    internal interface IEmployeeService
    {

        Task<SearchFiltersRequestModel> GetSearchInformation(); 
        Task<ICollection<EmployeeViewModel>> GetEmployees(SearchFiltersRequestModel searchFilters);

    }
}
