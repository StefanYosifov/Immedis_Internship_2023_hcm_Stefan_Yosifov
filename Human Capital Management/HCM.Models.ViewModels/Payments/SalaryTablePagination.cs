namespace HCM.Models.ViewModels.Payments
{
    using Common.Helpers;

    public class SalaryTablePagination
    {
        public SalaryTablePagination()
        {
            this.Employees = new List<SalaryTableModel>();
        }
        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public IList<SalaryTableModel> Employees { get; set; } = null!;

    }
}
