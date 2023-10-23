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
            this.Departments = new HashSet<DepartmentViewModel>();
            this.Genders = new HashSet<GenderViewModel>();
            this.Counties = new HashSet<CountryViewModel>();
        }

        public ICollection<DepartmentViewModel> Departments { get; set; }


        public ICollection<GenderViewModel> Genders { get; set; }

        public ICollection<CountryViewModel> Counties { get; set; }

        public string[] Sort { get; set; }

    }
}
