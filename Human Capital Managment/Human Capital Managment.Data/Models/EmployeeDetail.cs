using System;
using System.Collections.Generic;

namespace Human_Capital_Managment.Data.Models
{
    public partial class EmployeeDetail
    {
        public string EmployeeId { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int CountryOfBirthId { get; set; }
        public int CountryOfResidenceId { get; set; }
        public int? GenderId { get; set; }

        public virtual Country CountryOfBirth { get; set; } = null!;
        public virtual Country CountryOfResidence { get; set; } = null!;
        public virtual Gender? Gender { get; set; }
    }
}
