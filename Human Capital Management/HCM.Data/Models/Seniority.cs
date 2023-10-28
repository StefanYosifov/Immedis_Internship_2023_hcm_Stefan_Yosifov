namespace HCM.Data.Models
{
    using System.Collections.Generic;

    public partial class Seniority
    {
        public Seniority()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
