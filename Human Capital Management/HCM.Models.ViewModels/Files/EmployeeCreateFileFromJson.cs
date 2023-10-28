namespace HCM.Models.ViewModels.Files
{
    using Data.Models;
    using System.Text;
    using System.Text.Json;

    public class EmployeeCreateFileFromJson : IEmployeeCreateFile
    {
        public List<Employee> ProcessFile(byte[] fileContent)
        {
            string data = Encoding.UTF8.GetString(fileContent);
            var employees = JsonSerializer.Deserialize<List<Employee>>(data);
            return employees!;
        }
    }
}
