namespace HCM.Models.ViewModels.Files
{
    public class CreateEmployeeFileFactory
    {
        public IEmployeeCreateFile CreateEmployeeFile(string extension)
        {
            if (extension == ".json")
            {
                return new EmployeeCreateFileFromJson();
            }

            if (extension == ".xml")
            {
                return null;
            }

            return null;
        }
    }
}