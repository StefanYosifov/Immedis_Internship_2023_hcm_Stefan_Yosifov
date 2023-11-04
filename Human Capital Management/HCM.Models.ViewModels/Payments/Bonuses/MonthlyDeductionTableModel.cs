namespace HCM.Models.ViewModels.Payments.Bonuses
{
    public class MonthlyDeductionTableModel
    {
        public long DeductionId { get; set; }

        public decimal? DeductionAmount { get; set; }

        public DeductionReasonModel DeductionReason { get; set; }
    }
}