namespace HCM.API.Controllers.Payments
{
    using Common.Exceptions_Messages.Payments;

    using Core.Services.Department;
    using Core.Services.Payments;

    using Microsoft.AspNetCore.Mvc;

    using Models.ViewModels.Payments;
    using Models.ViewModels.Payments.Bonuses;
    using Models.ViewModels.Payments.Enums;
    using Models.ViewModels.Payments.Payroll;

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
            var salaryTableQueryOptionsModel = new SalaryTableQueryOptionsModel
            {
                Departments = await departmentService.GetDepartments(),
                Sort = Enum.GetNames(typeof(SalaryOptionsOrderModel))
            };

            return Ok(salaryTableQueryOptionsModel);
        }

        [HttpGet("salary/all/{page}")]
        public async Task<IActionResult> GetPaginatedSalaries(int page, [FromQuery] SalaryTableQueryModel query)
        {
            var result = await service.GetEmployeeSalaryInformation(page, query);
            return Ok(result);
        }

        [HttpGet("salary/info/{id:guid}")]
        public async Task<IActionResult> GetEmployeeSalaryInformation(string id)
        {
            try
            {
                var result = await service.GetEmployeeSalaryInformationById(id);
                return Ok(result);
            }
            catch (PaymentServiceExceptions e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("bonus/reasons")]
        public async Task<IActionResult> GetAllBonusReasons()
        {
            var result = await service.GetAllReasonsForBonuses();
            return Ok(result);
        }

        [HttpGet("deduction/reasons")]
        public async Task<IActionResult> GetAllDeductionReasons()
        {
            var result = await service.GetAllDeductionReasonsForDeduction();
            return Ok(result);
        }

        [HttpGet("monthly/additions")]
        public async Task<IActionResult> GetMonthlySalaryAdditionsByMonth(
            [FromQuery] TableBonusDeductionSearchModel model)
        {
            try
            {
                var result = await service.GetMonthlyAdditionTables(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("salary/change")]
        public async Task<IActionResult> ChangeSalary([FromQuery] SalaryChangeRequestModel model)
        {
            try
            {
                var result = await service.ChangeEmployeeSalary(model);
                return Ok(result);
            }
            catch (PaymentServiceExceptions e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return BadRequest(PaymentMessages.IssueUpdatingWithSalary);
            }
        }

        [HttpPost("bonus/add")]
        public async Task<IActionResult> AddBonus([FromBody] BonusAddModel model)
        {
            try
            {
                var result = await service.AddBonus(model);
                return Ok(result);
            }
            catch (PaymentServiceExceptions e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return BadRequest(PaymentMessages.IssueAddingBonus);
            }
        }

        [HttpPost("deduction/add")]
        public async Task<IActionResult> AddDeduction([FromBody] DeductionAddModel model)
        {
            try
            {
                var result = await service.AddDeduction(model);
                return Ok(result);
            }
            catch (PaymentServiceExceptions e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return BadRequest(PaymentMessages.IssueAddingDeduction);
            }
        }

        [HttpGet("payroll/pagination")]
        public async Task<IActionResult> GetPayrollPagination([FromQuery] PayRollSearchModel model)
        {
            var result = await service.GetPayrolls(model);
            return Ok(result);
        }

        [HttpPost("payroll/create")]
        public async Task<IActionResult> CreatePayroll([FromBody] PayrollCreateModel model)
        {
            try
            {
                var result = await service.CreatePayrollForDepartments(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(PaymentMessages.IssueAddingPayroll);
            }
        }

        [HttpGet("payroll/unpaid")]
        public async Task<IActionResult> GetUnpaidPayrolls()
        {
            var result = await service.GetAllUnpaidSalaries();
            return Ok(result);
        }

        [HttpPut("payroll/complete/{id}")]
        public async Task<IActionResult> CompletePayroll(int id)
        {
            try
            {
                var result = await service.CompletePayrollById(id);
                return Ok(result);
            }
            catch (PaymentServiceExceptions e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(PaymentMessages.IssueCompletingPayroll);
            }
        }

        [HttpDelete("payroll/remove/{id}")]
        public async Task<IActionResult> RemovePayroll(int id)
        {
            try
            {
                var result = await service.RemovePayrollById(id);
                return Ok(result);
            }
            catch (PaymentServiceExceptions e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(PaymentMessages.IssueCompletingPayroll);
            }
        }

    }
}