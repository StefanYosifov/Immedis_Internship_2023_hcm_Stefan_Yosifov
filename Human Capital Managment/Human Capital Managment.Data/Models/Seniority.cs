using System;
using System.Collections.Generic;

namespace Human_Capital_Managment.Data.Models
{
    public partial class Seniority
    {
        public Seniority()
        {
            Positions = new HashSet<Position>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Position> Positions { get; set; }
    }
}
