namespace HCM.Core.Services.Payments
{
    using Common.Helpers;

    using Models.ViewModels.Payments;

    public interface IPaymentService
    {

        Task<SalaryTablePagination> GetEmployeeSalaryInformation(int page, SalaryTableQueryModel query);

    }
}
