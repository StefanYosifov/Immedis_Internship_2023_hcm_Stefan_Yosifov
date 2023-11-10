namespace HCM.Models.ViewModels.Admin
{
    using Countries;

    using Data.Models;
    using Positions;

    public class AdminDepartmentsModel
    {

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int? EmployeeCapacity { get; set; }

        public int EmployeesCount { get; set; }

        public string? DepartmentImageUrl { get; set; }

        public string CountryName { get; set; }


    }
}
