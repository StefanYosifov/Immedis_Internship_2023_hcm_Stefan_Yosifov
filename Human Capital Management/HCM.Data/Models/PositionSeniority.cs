namespace HCM.Data.Models
{
    public class PositionSeniority
    {
        public int? PositionId { get; set; }
        public int? SeniorityId { get; set; }

        public virtual Position? Position { get; set; }
        public virtual Seniority? Seniority { get; set; }
    }
}