namespace HCM.API.Controllers.Payments
{
    using Core.Services.Department;
    using Core.Services.Payments;

    using Microsoft.AspNetCore.Mvc;
    using Models.ViewModels.Payments;
    using Models.ViewModels.Payments.Enums;

    [Route("/api/payments")]
    public class PaymentController : ApiController
    {
        private readonly IDepartmentService departmentService;
        private readonly IPaymentService service;

        public PaymentController(
            IDepartmentService departmentService, 
            IPaymentService service)
        {
            this.departmentService = departmentService;
            this.service = service;
        }

        [HttpGet("salary/options")]
        public async Task<IActionResult> GetSalarySortingOptions()
        {
            var salaryTableQueryOptionsModel = new SalaryTableQueryOptionsModel()
            {
                Departments = await departmentService.GetDepartments(),
                Sort = Enum.GetNames(typeof(SalaryOptionsOrderModel))
            };

            return Ok(salaryTableQueryOptionsModel);
        }

        [HttpGet("salary/all/{page}")]
        public async Task<IActionResult> GetPaginatedSalaries(int page,[FromQuery]SalaryTableQueryModel query)
        {
            var result = await service.GetEmployeeSalaryInformation(page, query);
            return Ok(result);
        }


    }
}
