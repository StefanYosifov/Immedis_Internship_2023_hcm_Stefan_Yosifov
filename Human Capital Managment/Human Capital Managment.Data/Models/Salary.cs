namespace Human_Capital_Managment.Data.Models
{
    public partial class Salary
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal? Bonus { get; set; }
        public string ContractId { get; set; } = null!;

        public virtual Contract? Contract { get; set; }
    }
}
