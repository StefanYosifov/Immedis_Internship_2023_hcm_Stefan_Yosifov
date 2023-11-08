namespace HCM.Models.ViewModels.Employees
{
    public class EmployeeEditPositionAndSeniority
    {
        public string EmployeeId { get; set; } = null!;

        public int PositionId { get; set; }

        public int SeniorityId { get; set; }
    }
}