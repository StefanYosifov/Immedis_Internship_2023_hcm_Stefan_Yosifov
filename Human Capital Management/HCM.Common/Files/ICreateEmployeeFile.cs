namespace HCM.Models.ViewModels.Files
{
    using Data.Models;

    public interface IEmployeeCreateFile
    {
        ICollection<Employee>ProcessFile(byte[] fileContent);
    }
}