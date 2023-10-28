namespace HCM.Data.Models
{
    using System.Collections.Generic;

    public partial class BonusesReason
    {
        public BonusesReason()
        {
            Bonuses = new HashSet<Bonuse>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Bonuse> Bonuses { get; set; }
    }
}
