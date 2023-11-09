namespace HCM.Models.ViewModels.Files
{
    using System.Text;
    using System.Text.Json;

    using Data.Models;

    public class EmployeeJSONFile: IEmployeeCreateFile
    {
        public ICollection<Employee> ProcessFile(byte[] fileContent)
        {
            var data = Encoding.UTF8.GetString(fileContent);
            var employees = JsonSerializer.Deserialize<List<Employee>>(data);
            return employees!;
        }
    }
}