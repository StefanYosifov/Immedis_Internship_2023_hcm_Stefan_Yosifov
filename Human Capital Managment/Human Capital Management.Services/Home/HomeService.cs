namespace Human_Capital_Management.Services.Home
{
    using System.Reflection.Metadata.Ecma335;

    using Human_Capital_Managment.Data;

    public class HomeService : IHomeService
    {

        private readonly ApplicationDbContext context;

        public HomeService(
            ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> HasConfirmedData(string userId)
        {
            var findEmployeeDetails=await context.EmployeeDetails.FindAsync(userId);

            return findEmployeeDetails != null;
        }
    }
}
