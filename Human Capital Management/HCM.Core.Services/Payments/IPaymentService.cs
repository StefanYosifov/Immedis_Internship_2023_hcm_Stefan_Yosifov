namespace HCM.Core.Services.Payments
{
    using Models.ViewModels.Payments;
    using Models.ViewModels.Payments.Bonuses;

    public interface IPaymentService
    {
        Task<SalaryTablePagination> GetEmployeeSalaryInformation(int page, SalaryTableQueryModel query);

        Task<SalaryChangeModel> GetEmployeeSalaryInformationById(string id);

        Task<ICollection<BonusReasonModel>> GetAllReasonsForBonuses();

        Task<ICollection<DeductionReasonModel>> GetAllDeductionReasonsForDeduction();

        Task<MonthlyBonusDeductionTableModel> GetMonthlySalaryAdditionsByMonth(TableBonusDeductionSearchModel model);
    }
}