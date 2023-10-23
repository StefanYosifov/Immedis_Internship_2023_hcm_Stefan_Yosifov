namespace HCM.API.Services.Services.Gender
{
    using HCM.Models.ViewModels.Genders;

    public interface IGenderService
    {
        Task<ICollection<GenderViewModel>> GetGenders();
    }
}
