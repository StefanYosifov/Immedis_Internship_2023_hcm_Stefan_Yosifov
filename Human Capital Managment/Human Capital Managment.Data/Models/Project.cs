using System;
using System.Collections.Generic;

namespace Human_Capital_Managment.Data.Models
{
    public partial class Project
    {
        public Project()
        {
            EmployeeProjects = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public byte[]? Image { get; set; }
        public string? Description { get; set; }
        public string ManagerId { get; set; } = null!;

        public virtual Employee? EmployeeId1 { get; set; }
        public virtual ICollection<Employee> EmployeeProjects { get; set; }
    }
}
