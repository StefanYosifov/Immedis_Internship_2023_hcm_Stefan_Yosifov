namespace HCM.Data.Models
{
    public class Status
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