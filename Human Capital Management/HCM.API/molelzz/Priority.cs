using System;
using System.Collections.Generic;

namespace HCM.API.molelzz
{
    public partial class Priority
    {
        public Priority()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string? PriorityName { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
