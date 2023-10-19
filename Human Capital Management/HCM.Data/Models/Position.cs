using System;
using System.Collections.Generic;

namespace HCM.Data.Models
{
    public partial class Position
    {
        public Position()
        {
            Employees = new HashSet<Employee>();
        }

        public byte Id { get; set; }
        public string Name { get; set; } = null!;
        public byte? DepartmentId { get; set; }

        public virtual Department? Department { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
