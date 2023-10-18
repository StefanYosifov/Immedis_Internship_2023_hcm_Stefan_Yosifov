namespace Human_Capital_Managment.ViewModels.Employee
{
    using CountryViewModels;

    using Data.Models;

    using Department;

    using Gender;

    using Status;

    public class SearchFiltersRequestModel
    {
        public SearchFiltersRequestModel()
        {
            this.Genders = new HashSet<GenderViewModel>();
            this.Countries = new HashSet<CountryViewModel>();
            this.Statuses=new HashSet<StatusViewModel>();
            this.Departments = new HashSet<DepartmentViewModel>();
        }

        public ICollection<GenderViewModel> Genders { get; set; }

        public ICollection<CountryViewModel> Countries { get; set; }

        public ICollection<StatusViewModel> Statuses { get; set; }

        public ICollection<DepartmentViewModel> Departments { get; set; }

    }
}
