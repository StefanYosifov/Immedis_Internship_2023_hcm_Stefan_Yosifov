namespace HCM.Models.ViewModels.Files
{
    using Data.Models;

    public interface IEmployeeCreateFile
    {
        List<Employee> ProcessFile(byte[] fileContent);
    }
}