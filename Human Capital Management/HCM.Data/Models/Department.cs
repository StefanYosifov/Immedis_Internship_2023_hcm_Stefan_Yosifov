namespace HCM.Data.Models
{
    using System.Collections.Generic;

    public partial class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
            Positions = new HashSet<Position>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? MaxPeopleCount { get; set; }
        public string? ImageUrl { get; set; }
        public int? CountryId { get; set; }

        public virtual Country? Country { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
    }
}
