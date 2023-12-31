﻿namespace HCM.Data.Models
{
    using History_and_Audit;

    public class Employee : IEntity
    {
        public Employee()
        {
            Id = new Guid().ToString();
            AuditLogs = new HashSet<AuditLog>();
            Bonuses = new HashSet<Bonuse>();
            Deductions = new HashSet<Deduction>();
            Payrolls = new HashSet<Payroll>();
            Tasks = new HashSet<Task>();
            EmployeeRoles = new HashSet<EmployeeRoles>();
            TaskIssuers = new HashSet<Task>();
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
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public int? SeniorityId { get; set; }

        public virtual Department? Department { get; set; }
        public virtual Gender? Gender { get; set; }
        public virtual Country? Nationality { get; set; }
        public virtual Position? Position { get; set; }
        public virtual Seniority? Seniority { get; set; }
        public virtual Salary? Salary { get; set; }
        public virtual ICollection<AuditLog> AuditLogs { get; set; }
        public virtual ICollection<Bonuse> Bonuses { get; set; }
        public virtual ICollection<Deduction> Deductions { get; set; }
        public virtual ICollection<Payroll> Payrolls { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<Task> TaskIssuers { get; set; }

        public virtual ICollection<EmployeeRoles> EmployeeRoles { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
    }
}