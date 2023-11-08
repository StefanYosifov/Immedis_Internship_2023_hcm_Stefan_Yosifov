namespace HCM.Core.Services.Countries
{
    using Models.ViewModels.Countries;

    public interface ICountryService
    {
        Task<ICollection<CountryViewModel>> GetCountries();

        Task<Dictionary<int,decimal?>> GetCountriesTaxRates();
    }
}