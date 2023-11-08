namespace HCM.Models.ViewModels.Payments.Payroll
{
    using Departments;

    public class PayrollPaginationModel
    {
        public PayrollPaginationModel()
        {
            this.Payroll = new HashSet<PayrollModel>();
            this.Departments = new HashSet<DepartmentViewModel>();
        }

        public int Page { get; set; }

        public int TotalPages { get; set; }

        public ICollection<PayrollModel> Payroll { get; set; }

        public ICollection<DepartmentViewModel> Departments { get; set; }

    }
}
