namespace HCM.Data.History_and_Audit
{
    internal interface IEntity
    {

        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }

    }
}
