namespace Human_Capital_Managment.Data.Models
{
    using System;
    using System.Collections.Generic;

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
        public string? PhoneNumber { get; set; }
        public string? CountryOfBirth { get; set; }
        public string? CountryOfResidence { get; set; }
        public DateTime? JoinedAt { get; set; }
        public int? DepartmentId { get; set; }
        public int? StatusId { get; set; }
        public string? ContractId { get; set; }
        public string? PaymentId { get; set; }
        public int? PositionId { get; set; }
        public string? EmployeeDetailsId { get; set; }
        public int? ProjectId { get; set; }

        public virtual Department? Department { get; set; }
        public virtual Project Id1 { get; set; } = null!;
        public virtual EmployeeDetail IdNavigation { get; set; } = null!;
        public virtual SalaryPayment? Payment { get; set; }
        public virtual Position? Position { get; set; }
        public virtual Project? Project { get; set; }
        public virtual EmployeeStatus? Status { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
