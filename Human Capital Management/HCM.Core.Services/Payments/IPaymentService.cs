namespace HCM.Core.Services.Payments
{
    using Models.ViewModels.Payments;
    using Models.ViewModels.Payments.Bonuses;
    using Models.ViewModels.Payments.Payroll;

    public interface IPaymentService
    {
        Task<SalaryTablePagination> GetEmployeeSalaryInformation(int page, SalaryTableQueryModel query);

        Task<SalaryChangeModel> GetEmployeeSalaryInformationById(string id);

        Task<ICollection<BonusReasonModel>> GetAllReasonsForBonuses();

        Task<ICollection<DeductionReasonModel>> GetAllDeductionReasonsForDeduction();

        Task<MonthlyBonusDeductionTableModel> GetMonthlyAdditionTables(TableBonusDeductionSearchModel model);

        Task<string> ChangeEmployeeSalary(SalaryChangeRequestModel model);

        Task<string> AddBonus(BonusAddModel model);

        Task<string> AddDeduction(DeductionAddModel model);

        Task<PayrollPaginationModel> GetPayrolls(int id, PayRollSearchModel model);

        Task<string> CreatePayrollForDepartments(PayrollCreateModel model);

        Task<ICollection<PayrollAllUnpaidSalaries>> GetAllUnpaidSalaries();

        Task<string> CompletePayrollById(int payrollId);

        Task<string> RemovePayrollById(int payrollId);

        Task<ICollection<PayrollEmployeeDetails>> GetPayRollDetailsByEmployeeId(string employeeId);
    }
}