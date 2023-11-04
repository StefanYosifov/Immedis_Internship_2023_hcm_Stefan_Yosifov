namespace HCM.Models.ViewModels.Departments
{
    using Countries;

    using Positions;

    public class DepartmentDetailsViewModel
    {
        public DepartmentDetailsViewModel()
        {
            DepartmentEmployees = new HashSet<DepartmentEmployeesModel>();
            PositionsInDepartment = new HashSet<DepartmentGetPositionsModel>();
            AvailablePositionsCollection = new HashSet<PositionViewModel>();
            Countries = new HashSet<CountryViewModel>();
        }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int? EmployeeCapacity { get; set; }

        public int EmployeesCount { get; set; }

        public string DepartmentImageUrl { get; set; }

        public string CountryName { get; set; }

        public int AverageSalary { get; set; }

        public ICollection<CountryViewModel> Countries { get; set; }

        public ICollection<DepartmentEmployeesModel?>? DepartmentEmployees { get; set; }

        public ICollection<DepartmentGetPositionsModel> PositionsInDepartment { get; set; }

        public ICollection<PositionViewModel> AvailablePositionsCollection { get; set; }
    }
}