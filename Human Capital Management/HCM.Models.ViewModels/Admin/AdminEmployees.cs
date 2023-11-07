namespace HCM.Models.ViewModels.Admin
{
    using Employees;

    public class AdminEmployees
    {
        public AdminEmployees()
        {
            this.RegularEmployees = new HashSet<EmployeeTableDataModel>();
            this.HrEmployees=new HashSet<EmployeeTableDataModel>();
        }

        public ICollection<EmployeeTableDataModel> RegularEmployees { get; set; }

        public ICollection<EmployeeTableDataModel> HrEmployees { get; set; }

    }
}
