namespace HCM.Data.History_and_Audit
{
    internal interface ICreationEntity
    {
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
    }
}
