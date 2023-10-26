namespace HCM.Data.Models
{
    public class Role
    {
        public Role()
        {
            EmployeeRoles = new HashSet<EmployeeRoles>();
        }

        public byte Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<EmployeeRoles> EmployeeRoles { get; set; }
    }
}
