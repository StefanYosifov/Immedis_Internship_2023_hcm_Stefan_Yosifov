namespace HCM.Models.ViewModels.Departments
{
    public class DepartmentGetAllModel
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = null!;

        public string? DepartmentCountry { get; set; }

        public string? DepartmentImageUrl { get; set; }

        public int DepartmentEmployeeCount { get; set; }

        public int? DepartmentTotalEmployeeCapacity { get; set; }
    }
}