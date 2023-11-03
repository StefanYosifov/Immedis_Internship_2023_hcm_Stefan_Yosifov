namespace HCM.Models.ViewModels.Employees
{
    using Countries;

    using Departments;

    using Genders;

    using Positions;

    using Seniorities;

    public class EmployeeCreateDropDownOptions
    {
        public EmployeeCreateDropDownOptions()
        {
            Departments = new HashSet<DepartmentViewModel>();
            Genders = new HashSet<GenderViewModel>();
            Countries = new HashSet<CountryViewModel>();
        }

        public ICollection<CountryViewModel> Countries { get; set; }
        public ICollection<GenderViewModel> Genders { get; set; }
        public ICollection<DepartmentViewModel> Departments { get; set; }

        public ICollection<PositionViewModel> Positions { get; set; }

        public ICollection<SeniorityViewModel> Seniority { get; set; }
    }
}