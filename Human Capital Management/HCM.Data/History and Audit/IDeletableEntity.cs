namespace HCM.Data.History_and_Audit
{
    internal interface IDeletableEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
    }
}
