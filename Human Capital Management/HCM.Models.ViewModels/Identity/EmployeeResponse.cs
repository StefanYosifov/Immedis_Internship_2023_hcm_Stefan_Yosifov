namespace HCM.Models.ViewModels.Identity
{
    using Microsoft.AspNetCore.Identity;

    public class EmployeeResponse
    {
        public EmployeeResponse()
        {
            Roles = new HashSet<string>();
        }
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public ICollection<string> Roles { get; set; }

    }
}
