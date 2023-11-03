namespace HCM.Models.ViewModels.Departments
{
    public class DepartmentGetPositionsModel
    {
        public int PositionId { get; set; }

        public string PositionName { get; set; } = null!;

        public int EmployeesWithPositionCount { get; set; }
    }
}