namespace HCM.Models.ViewModels.Payments.Payroll
{
    public class PayRollSearchModel
    {
        public int Page { get; set; }

        public string? StartDate { get; set; }

        public string? EndDate { get; set; }

        public int DepartmentId { get; set; }

    }
}
