namespace HCM.Data.Models
{
    public class Country
    {
        public Country()
        {
            Departments = new HashSet<Department>();
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Iso { get; set; } = null!;

        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}