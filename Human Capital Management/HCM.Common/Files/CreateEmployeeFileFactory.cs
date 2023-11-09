namespace HCM.Models.ViewModels.Files
{
    public class CreateEmployeeFileFactory
    {
        public IEmployeeCreateFile CreateEmployeeFile(string extension)
        {
            if (extension == ".json")
            {
                return new EmployeeJSONFile();
            }

            if (extension == ".xml")
            {
                return new EmployeeXMLFile();
            }

            return null;
        }
    }
}