namespace HCM.Models.ViewModels.Countries
{
    using Departments;

    using Genders;

    public class EmployeeCreationalOptions
    {
        public EmployeeCreationalOptions()
        {
            this.Genders = new HashSet<GenderViewModel>();
            this.Countries = new HashSet<CountryViewModel>();
            this.Departments = new HashSet<DepartmentViewModel>();
        }


        public ICollection<GenderViewModel> Genders { get; set; }

        public ICollection<CountryViewModel> Countries { get; set; }

        public ICollection<DepartmentViewModel> Departments { get; set; }
    }
}
