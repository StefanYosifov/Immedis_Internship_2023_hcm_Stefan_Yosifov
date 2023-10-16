namespace Human_Capital_Managment.Data.Models
{
    using System.Collections.Generic;

    public partial class Gender
    {
        public Gender()
        {
            EmployeeDetails = new HashSet<EmployeeDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<EmployeeDetail> EmployeeDetails { get; set; }
    }
}
