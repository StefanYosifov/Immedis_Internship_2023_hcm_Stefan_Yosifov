namespace HCM.Models.ViewModels.Countries
{
    using System.ComponentModel.DataAnnotations;

    public class CountryViewModel
    {
        [Required] public int Id { get; set; }

        [Required] public string Name { get; set; } = null!;
    }
}