﻿using System;
using System.Collections.Generic;

namespace Human_Capital_Managment.Data.Models
{
    public partial class Status
    {
        public Status()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
