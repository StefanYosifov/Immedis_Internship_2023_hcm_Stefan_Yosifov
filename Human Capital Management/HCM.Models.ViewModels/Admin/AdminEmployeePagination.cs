namespace HCM.Models.ViewModels.Admin
{
    public class AdminEmployeePagination
    {
        public AdminEmployeePagination()
        {
            this.Employees = new HashSet<AdminEmployeeModel>();
        }

        public ICollection<AdminEmployeeModel> Employees { get; set; }

        public ICollection<RoleViewModel> Roles { get; set; }

        public int Page { get; set; }

        public int TotalPages { get; set; }

    }
}
