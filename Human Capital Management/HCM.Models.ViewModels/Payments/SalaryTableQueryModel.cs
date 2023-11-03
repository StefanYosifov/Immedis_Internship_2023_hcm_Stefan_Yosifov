namespace HCM.Models.ViewModels.Payments
{
    using Enums;

    public class SalaryTableQueryModel
    {
        public string? Search { get; set; }
        public int DepartmentId { get; set; }

        public int PositionId { get; set; }

        public int SeniorityId { get; set; }

        public SalaryOptionsOrderModel Sort { get; set; }

    }
}
