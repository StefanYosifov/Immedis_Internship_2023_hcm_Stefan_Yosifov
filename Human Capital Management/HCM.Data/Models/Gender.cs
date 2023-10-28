namespace HCM.Data.Models
{
    using System.Collections.Generic;

    public partial class Gender
    {
        public Gender()
        {
            Employees = new HashSet<Employee>();
        }

        public byte Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
