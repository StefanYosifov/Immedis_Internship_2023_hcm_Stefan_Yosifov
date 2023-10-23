namespace HCM.API.Services.Services.Countries
{
    using Models.ViewModels.Countries;

    public interface ICountryService
    {
        Task<ICollection<CountryViewModel>> GetCountries();
    }
}
