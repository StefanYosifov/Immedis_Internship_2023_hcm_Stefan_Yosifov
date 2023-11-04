namespace HCM.Models.ViewModels.Payments
{
    using Departments;

    using Positions;

    using Seniorities;

    public class SalaryTableQueryOptionsModel
    {
        public SalaryTableQueryOptionsModel()
        {
            Departments = new HashSet<DepartmentViewModel>();
            Positions = new HashSet<PositionViewModel>();
            Seniorities = new HashSet<SeniorityViewModel>();
        }

        public ICollection<DepartmentViewModel> Departments { get; set; }

        public ICollection<PositionViewModel> Positions { get; set; }

        public ICollection<SeniorityViewModel> Seniorities { get; set; }

        public string[] Sort { get; set; } = null!;
    }
}