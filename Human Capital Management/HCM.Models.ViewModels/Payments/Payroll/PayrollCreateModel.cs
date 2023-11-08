namespace HCM.Models.ViewModels.Payments.Payroll
{
    public class PayrollCreateModel
    {
        public PayrollCreateModel()
        {
            this.DepartmentIds=new List<int>();
        }

        public string? StartDate { get; set; }

        public string? EndDate { get; set;}

        public bool IncludeBonuses { get; set; }

        public bool IncludeDeductions { get; set; }

        public decimal Percentage { get; set; }

        public ICollection<int> DepartmentIds { get; set; }

    }
}
