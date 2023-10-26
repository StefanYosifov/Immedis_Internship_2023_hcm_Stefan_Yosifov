namespace HCM.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class EmployeeRoles
    {

        [ForeignKey(nameof(Employee))]
        public string EmployeeId { get; set; }

        public Employee Employee { get; set; }

        [ForeignKey(nameof(Role))]
        public byte RoleId { get; set; }

        public Role Role { get; set; }

    }
}
