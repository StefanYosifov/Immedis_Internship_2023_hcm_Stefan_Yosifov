namespace HCM.Models.ViewModels.Payments.Bonuses
{
    public class MonthlyBonusTableModel
    {
        public long BonusId { get; set; }

        public decimal DeductionAmount { get; set; }

        public BonusReasonModel BonusReason { get; set; }
    }
}