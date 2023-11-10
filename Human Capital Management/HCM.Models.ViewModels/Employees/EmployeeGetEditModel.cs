namespace HCM.Models.ViewModels.Employees
{
    using Countries;

    using Departments;

    using Positions;

    using Seniorities;

    using Tasks;

    public class EmployeeGetEditModel
    {
        public EmployeeGetEditModel()
        {
            Deparments = new HashSet<DepartmentViewModel>();
            Positions = new HashSet<PositionViewModel>();
            Senioritys = new HashSet<SeniorityViewModel>();
        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public int NationalityId { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public int SeniorityId { get; set; }
        public SearchTaskOptionsModel Tasks { get; set; }
        public ICollection<CountryViewModel> Nationalities { get; set; }

        public ICollection<DepartmentViewModel> Deparments { get; set; }

        public ICollection<PositionViewModel> Positions { get; set; }

        public ICollection<SeniorityViewModel> Senioritys { get; set; }

    }
}