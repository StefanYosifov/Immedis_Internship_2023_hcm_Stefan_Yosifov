using System;
using System.Collections.Generic;

namespace HCM.API.molelzz
{
    public partial class Status
    {
        public Status()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string? StatusName { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
