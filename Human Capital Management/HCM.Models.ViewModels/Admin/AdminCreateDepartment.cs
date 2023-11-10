namespace HCM.Models.ViewModels.Admin
{
    using System.ComponentModel.DataAnnotations;

    public class AdminCreateDepartment
    {
        [Required] public string Name { get; set; } = null!;

        [Required] public int MaxPeople { get; set; }

        [Required] public string ImageUrl { get; set; } = null!;

        [Required] public int CountryId { get; set; }
    }
}