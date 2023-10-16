﻿namespace Human_Capital_Managment.Data.Models
{
    using System.Collections.Generic;

    public partial class Country
    {
        public Country()
        {
            EmployeeDetailCountryOfBirths = new HashSet<EmployeeDetail>();
            EmployeeDetailCountryOfResidences = new HashSet<EmployeeDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<EmployeeDetail> EmployeeDetailCountryOfBirths { get; set; }
        public virtual ICollection<EmployeeDetail> EmployeeDetailCountryOfResidences { get; set; }
    }
}