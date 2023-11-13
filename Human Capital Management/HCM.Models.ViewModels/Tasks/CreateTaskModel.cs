namespace HCM.Models.ViewModels.Tasks
{
    using System.ComponentModel.DataAnnotations;

    public class CreateTaskModel
    {
        [Required] public string EmployeeId { get; set; } = null!;

        [Required] public string TaskName { get; set; } = null!;

        [Required] public string Description { get; set; } = null!;

        [Required] public DateTime DueDate { get; set; }

        [Required] public int PriorityId { get; set; }
    }
}