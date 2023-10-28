namespace HCM.Data.Models
{
    using System.Collections.Generic;

    public partial class Role
    {
        public Role()
        {
            EmployeeRoles = new HashSet<EmployeeRoles>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<EmployeeRoles> EmployeeRoles { get; set; }
    }
}
