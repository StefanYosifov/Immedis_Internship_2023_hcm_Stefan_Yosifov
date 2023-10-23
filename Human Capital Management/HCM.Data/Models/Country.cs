using System;
using System.Collections.Generic;

namespace HCM.Data.Models
{
    public partial class Country
    {
        public Country()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Iso { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
