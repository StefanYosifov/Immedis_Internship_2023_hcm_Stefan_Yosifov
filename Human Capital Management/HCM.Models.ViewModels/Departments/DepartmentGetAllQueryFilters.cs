﻿namespace HCM.Models.ViewModels.Departments
{
    using Countries;

    public class DepartmentGetAllQueryFilters
    {
        public ICollection<CountryViewModel> Countries { get; set; }

        public string[] Sort { get; set; }
    }
}