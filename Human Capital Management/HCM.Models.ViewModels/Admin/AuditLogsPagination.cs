namespace HCM.Models.ViewModels.Admin
{
    public class AuditLogsPagination
    {
        public AuditLogsPagination()
        {
            this.AuditLogs = new HashSet<AuditLogs>();
        }

        public int Page { get; set; }

        public int TotalPages { get; set; }

        public ICollection<AuditLogs> AuditLogs { get; set; }

    }
}
