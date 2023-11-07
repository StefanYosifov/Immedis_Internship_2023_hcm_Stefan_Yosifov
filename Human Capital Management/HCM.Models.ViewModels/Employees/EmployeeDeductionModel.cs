namespace HCM.Models.ViewModels.Employees
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EmployeeDeductionModel
    {
        public string EmployeeId { get; set; } = null!;

        public string EmployeeName { get; set; } = null!;

        public decimal? DeductionAmount { get; set; }

        public int DeductionCount { get; set; }
    }
}
