namespace HCM.Models.ViewModels.Payments
{
    using Departments;

    using Enums;

    using Positions;

    using Seniorities;

    public class SalaryTableQueryOptionsModel
    {
        public SalaryTableQueryOptionsModel()
        {
            this.Departments = new HashSet<DepartmentViewModel>();
            this.Positions = new HashSet<PositionViewModel>();
            this.Seniorities = new HashSet<SeniorityViewModel>();
        }

        public ICollection<DepartmentViewModel> Departments { get; set; }

        public ICollection<PositionViewModel> Positions { get; set; }

        public ICollection<SeniorityViewModel> Seniorities { get; set; }

        public string[] Sort { get; set; } = null!;

    }
}
