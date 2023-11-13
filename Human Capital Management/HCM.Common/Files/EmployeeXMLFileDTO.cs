namespace HCM.Common.Files
{
    using System.Xml.Serialization;

    [XmlRoot("Employees")]
    public class EmployeesXMLFileDTO
    {
        [XmlElement("Employee")]
        public List<EmployeeXmlFileDto> Employees { get; set; }
    }

    public class EmployeeXmlFileDto
    {
        [XmlElement("Id")]
        public string Id { get; set; }

        [XmlElement("Username")]
        public string Username { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }

        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [XmlElement("LastName")]
        public string LastName { get; set; }

        [XmlElement("BirthDate")]
        public DateTime? BirthDate { get; set; }

        [XmlElement("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [XmlElement("NationalityId")]
        public int? NationalityId { get; set; }

        [XmlElement("GenderId")]
        public byte? GenderId { get; set; }

        [XmlElement("DepartmentId")]
        public int? DepartmentId { get; set; }
        [XmlElement("PositionId")]
        public int? PositionId { get; set; }
        [XmlElement("SeniorityId")]
        public int? SeniorityId { get; set; }

        [XmlElement("PasswordHash")] public string PasswordHash { get; set; } = null!;
    }
}