using System;
using System.Collections.Generic;

namespace Human_Capital_Managment.Data.Models
{
    public partial class Position
    {
        public Position()
        {
            Departments = new HashSet<Department>();
            Employees = new HashSet<Employee>();
            Seniorities = new HashSet<Seniority>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int SeniorityId { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<Seniority> Seniorities { get; set; }
    }
}
