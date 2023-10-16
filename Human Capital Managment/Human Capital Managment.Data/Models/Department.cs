using System;
using System.Collections.Generic;

namespace Human_Capital_Managment.Data.Models
{
    public partial class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string DepartmentName { get; set; } = null!;
        public int? PositionId { get; set; }

        public virtual Position? Position { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
