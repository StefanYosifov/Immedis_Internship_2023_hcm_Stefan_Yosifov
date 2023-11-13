namespace HCM.Models.ViewModels.Files
{
    using Data.Models;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;

    using Common.Files;

    internal class EmployeeXMLFile : IEmployeeCreateFile
    {

        public ICollection<Employee> ProcessFile(byte[] fileContent)
        {
            var data = Encoding.UTF8.GetString(fileContent);
            IList<Employee> employees = new List<Employee>();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Employees");
            XmlSerializer serializer = new XmlSerializer(typeof(EmployeesXMLFileDTO), xmlRoot); 

            StringReader reader = new StringReader(data);
            EmployeesXMLFileDTO employeesDto = (EmployeesXMLFileDTO)serializer.Deserialize(reader);

            foreach (var employeedDto in employeesDto.Employees) //Redundant nesting I believe
            {
                var employee=new Employee()
                {
                    Id = Guid.NewGuid().ToString(),
                    DepartmentId = employeedDto.DepartmentId,
                    BirthDate = employeedDto.BirthDate,
                    GenderId = employeedDto.GenderId,
                    Email = employeedDto.Email,
                    FirstName = employeedDto.FirstName,
                    LastName = employeedDto.LastName,
                    NationalityId = employeedDto.NationalityId,
                    PositionId = employeedDto.PositionId,
                    SeniorityId = employeedDto.SeniorityId,
                    Username = employeedDto.Username,
                    PhoneNumber = employeedDto.PhoneNumber,
                    PasswordHash = employeedDto.PasswordHash
                };
                employees.Add(employee);
            }
            

            return employees;



        }

        public static T[] DeserializeXml<T>(string xmlString, string rootElement)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T[]), new XmlRootAttribute(rootElement));
            using var stringReader = new StringReader(xmlString);

            return (T[])xmlSerializer.Deserialize(stringReader);
        }
    }
}
