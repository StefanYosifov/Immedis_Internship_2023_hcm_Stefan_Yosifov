namespace HCM.Core.Services.Gender
{
    using Models.ViewModels.Genders;

    public interface IGenderService
    {
        Task<ICollection<GenderViewModel>> GetGenders();
    }
}
