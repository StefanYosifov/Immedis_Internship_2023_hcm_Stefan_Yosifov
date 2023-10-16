using System;
using System.Collections.Generic;

namespace Human_Capital_Managment.Data.Models
{
    public partial class ContractAction
    {
        public ContractAction()
        {
            ContractHistories = new HashSet<ContractHistory>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<ContractHistory> ContractHistories { get; set; }
    }
}
