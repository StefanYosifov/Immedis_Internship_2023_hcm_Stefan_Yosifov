namespace Human_Capital_Management.Services.Home
{
    public interface IHomeService
    {

        Task<bool> HasConfirmedData(string userId);

    }
}
