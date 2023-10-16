using System;
using System.Collections.Generic;

namespace Human_Capital_Managment.Data.Models
{
    public partial class Contract
    {
        public Contract()
        {
            ContractHistories = new HashSet<ContractHistory>();
        }

        public string Id { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Details { get; set; }
        public string? EmployeeId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? SignedOn { get; set; }
        public DateTime? TerminatedOn { get; set; }
        public bool? IsSigned { get; set; }
        public bool? IsTerminated { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual Salary IdNavigation { get; set; } = null!;
        public virtual ICollection<ContractHistory> ContractHistories { get; set; }
    }
}
