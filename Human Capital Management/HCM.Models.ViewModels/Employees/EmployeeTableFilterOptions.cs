namespace HCM.Models.ViewModels.Employees
{
    using Countries;

    using Departments;

    using Enum;

    using Genders;

    public class EmployeeTableFilterOptions
    {
        public EmployeeTableFilterOptions()
        {
            Departments = new HashSet<DepartmentViewModel>();
            Genders = new HashSet<GenderViewModel>();
            Counties = new HashSet<CountryViewModel>();
        }

        public ICollection<DepartmentViewModel> Departments { get; set; }


        public ICollection<GenderViewModel> Genders { get; set; }

        public ICollection<CountryViewModel> Counties { get; set; }

        public string[] Sort { get; set; }

    }
}
