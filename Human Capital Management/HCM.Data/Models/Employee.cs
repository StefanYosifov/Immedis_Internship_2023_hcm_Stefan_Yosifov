using System;
using System.Collections.Generic;

namespace HCM.Data.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Payrolls = new HashSet<Payroll>();
            Roles = new HashSet<Role>();
        }

        public string Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? PhoneNumber { get; set; }
        public int? NationalityId { get; set; }
        public byte? GenderId { get; set; }
        public byte? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual Department? Department { get; set; }
        public virtual Gender? Gender { get; set; }
        public virtual Country? Nationality { get; set; }
        public virtual Position? Position { get; set; }
        public virtual Salary? Salary { get; set; }
        public virtual ICollection<Payroll> Payrolls { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
