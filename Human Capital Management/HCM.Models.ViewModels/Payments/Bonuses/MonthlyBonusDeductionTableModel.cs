namespace HCM.Models.ViewModels.Payments.Bonuses
{
    public class MonthlyBonusDeductionTableModel
    {
        public MonthlyBonusDeductionTableModel()
        {
            Bonuses = new HashSet<MonthlyBonusTableModel>();
            Deductions = new HashSet<MonthlyDeductionTableModel>();
        }

        public ICollection<MonthlyBonusTableModel> Bonuses { get; set; }

        public ICollection<MonthlyDeductionTableModel> Deductions { get; set; }
    }
}