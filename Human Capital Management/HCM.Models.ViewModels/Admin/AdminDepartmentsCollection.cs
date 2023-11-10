namespace HCM.Models.ViewModels.Admin
{
    using HCM.Models.ViewModels.Countries;

    public class AdminDepartmentsCollection
    {

        public AdminDepartmentsCollection()
        {
            this.Departments=new HashSet<AdminDepartmentsModel>();
            this.Countries = new HashSet<CountryViewModel>();
        }

        public ICollection<AdminDepartmentsModel> Departments { get; set; }

        public ICollection<CountryViewModel> Countries { get; set; }

    }
}
