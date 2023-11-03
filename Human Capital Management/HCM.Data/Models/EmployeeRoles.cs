namespace HCM.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class EmployeeRoles
    {
        [ForeignKey(nameof(Employee))] public string EmployeeId { get; set; } = null!;

        public Employee Employee { get; set; } = null!;

        [ForeignKey(nameof(Role))] public int RoleId { get; set; }

        public Role Role { get; set; } = null!;
    }
}