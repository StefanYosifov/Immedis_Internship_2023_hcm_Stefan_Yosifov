﻿namespace HCM.Data.Models
{
    public class Seniority
    {
        public Seniority()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
    }
}