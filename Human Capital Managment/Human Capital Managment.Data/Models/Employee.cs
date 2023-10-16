using System;
using System.Collections.Generic;

namespace Human_Capital_Managment.Data.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Id=Guid.NewGuid().ToString();
            Contracts = new HashSet<Contract>();
            InverseManager = new HashSet<Employee>();
            Roles = new HashSet<Role>();
        }

        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime? JoinedAt { get; set; }
        public int? DepartmentId { get; set; }
        public int? StatusId { get; set; }
        public string? ContractId { get; set; }
        public string? PaymentId { get; set; }
        public int? PositionId { get; set; }
        public string? EmployeeDetailsId { get; set; }
        public int? ProjectId { get; set; }
        public string? ManagerId { get; set; }

        public virtual Department? Department { get; set; }
        public virtual Project Id1 { get; set; } = null!;
        public virtual EmployeeDetail IdNavigation { get; set; } = null!;
        public virtual Employee? Manager { get; set; }
        public virtual SalaryPayment? Payment { get; set; }
        public virtual Position? Position { get; set; }
        public virtual Project? Project { get; set; }
        public virtual EmployeeStatus? Status { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<Employee> InverseManager { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
