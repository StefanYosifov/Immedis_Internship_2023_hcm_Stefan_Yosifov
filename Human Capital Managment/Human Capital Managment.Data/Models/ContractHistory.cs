namespace Human_Capital_Managment.Data.Models
{
    using System;

    public partial class ContractHistory
    {
        public int Id { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangedBy { get; set; } = null!;
        public int? ActionId { get; set; }
        public string ContractId { get; set; } = null!;

        public virtual ContractAction? Action { get; set; }
        public virtual Contract Contract { get; set; } = null!;
    }
}
