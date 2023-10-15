using System;
using System.Collections.Generic;

namespace Human_Capital_Managment.Data.Models2
{
    public partial class SalaryPayment
    {
        public SalaryPayment()
        {
            Employees = new HashSet<Employee>();
        }

        public string Id { get; set; } = null!;
        public DateTime PaidOn { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Reason { get; set; }
        public string EmployeeId { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
