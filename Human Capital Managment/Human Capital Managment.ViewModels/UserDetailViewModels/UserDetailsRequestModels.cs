namespace Human_Capital_Managment.ViewModels.UserDetailViewModels
{
    using System.ComponentModel.DataAnnotations;

    using Data.Models;

    public class UserDetailsRequestModels
    {

        public UserDetailsRequestModels()
        {
            this.CountriesOfBirth = new List<Country>();
            this.CountriesOfResidence =new List<Country>();
            this.Genders=new List<Gender>();
        }

        [Required]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public IEnumerable<Country> CountriesOfBirth { get; set; }
        [Required]
        public IEnumerable<Country>  CountriesOfResidence { get; set; }
        [Required]
        public IEnumerable<Gender> Genders { get; set; }

    }
}
