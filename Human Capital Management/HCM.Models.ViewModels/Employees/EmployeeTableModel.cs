namespace HCM.Models.ViewModels.Employees
{
    public class EmployeeTableModel
    {
        public EmployeeTableModel()
        {
            this.Employees = new HashSet<EmployeeTableDataModel>();
        }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public ICollection<EmployeeTableDataModel> Employees { get; set; }
    }
}