﻿using System;
using System.Collections.Generic;

namespace HCM.API.molelzz
{
    public partial class Position
    {
        public Position()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? DepartmentId { get; set; }

        public virtual Department? Department { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
