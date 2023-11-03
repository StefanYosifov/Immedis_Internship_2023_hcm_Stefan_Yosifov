namespace HCM.Models.ViewModels.Identity
{
    public class Response
    {
        public Response()
        {
            Claims = new Dictionary<string, string>();
        }

        public EmployeeResponse Employee { get; set; }
        public string JwtToken { get; set; }

        public Dictionary<string, string> Claims { get; set; }

        public bool isValid => JwtToken.Length > 0;
    }
}