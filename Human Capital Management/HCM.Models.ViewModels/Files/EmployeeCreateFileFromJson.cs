namespace HCM.Models.ViewModels.Files
{
    using System.Text;
    using System.Text.Json;

    using Data.Models;

    public class EmployeeCreateFileFromJson : IEmployeeCreateFile
    {
        public List<Employee> ProcessFile(byte[] fileContent)
        {
            var data = Encoding.UTF8.GetString(fileContent);
            var employees = JsonSerializer.Deserialize<List<Employee>>(data);
            return employees!;
        }
    }
}