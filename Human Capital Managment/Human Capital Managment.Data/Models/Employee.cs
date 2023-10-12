using System;
using System.Collections.Generic;

namespace Human_Capital_Managment.Data.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Contracts = new HashSet<Contract>();
            Roles = new HashSet<Role>();
        }

        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string CountryOfBirth { get; set; } = null!;
        public string CountryOfResidence { get; set; } = null!;
        public DateTime? JoinedAt { get; set; }
        public int? DepartmentId { get; set; }
        public int? StatusId { get; set; }
        public int? GenderId { get; set; }
        public string? ContractId { get; set; }
        public string? PaymentId { get; set; }
        public int? PositionId { get; set; }

        public virtual Department? Department { get; set; }
        public virtual Gender? Gender { get; set; }
        public virtual Payment? Payment { get; set; }
        public virtual Position? Position { get; set; }
        public virtual Status? Status { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
