namespace HCM.Models.ViewModels.Departments
{
    public class DepartmentEditDetails
    {
        public int DepartmentId { get; set; }

        public string Name { get; set; } = null!;

        public string ImageURL { get; set; } = null!;

        public int CountryId { get; set; }

        public int MaxPeopleCount { get; set; }
    }
}