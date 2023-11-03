namespace HCM.Models.ViewModels.Departments
{
    using Enums;

    public class DepartmentSendQueryFilters
    {
        public string? Search { get; set; }

        public int CountryId { get; set; }

        public DepartmentSortSearch Sort { get; set; }
    }
}