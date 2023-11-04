namespace HCM.Models.ViewModels.Payments
{
    public class SalaryTablePagination
    {
        public SalaryTablePagination()
        {
            Employees = new List<SalaryTableModel>();
        }

        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public IList<SalaryTableModel> Employees { get; set; } = null!;
    }
}