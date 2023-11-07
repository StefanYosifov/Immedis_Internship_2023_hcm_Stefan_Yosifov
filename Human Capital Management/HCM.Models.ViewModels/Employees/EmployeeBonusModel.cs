namespace HCM.Models.ViewModels.Employees
{
    public class EmployeeBonusModel
    {
        public string EmployeeId { get; set; } = null!;

        public string EmployeeName { get; set; } = null!;

        public decimal BonusAmount { get; set; }

        public int BonusesCount { get; set; }

    }
}
