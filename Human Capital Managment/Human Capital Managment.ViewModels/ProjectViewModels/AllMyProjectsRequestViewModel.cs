namespace Human_Capital_Managment.ViewModels.ProjectViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class AllMyProjectsRequestViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public byte[]? Image { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public int EmployeeCount { get; set; }
    }
}
