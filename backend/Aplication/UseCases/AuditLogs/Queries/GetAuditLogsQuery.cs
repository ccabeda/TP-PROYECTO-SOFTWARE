namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Queries
{
    public record GetAuditLogsQuery
    {
        public int? UserId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
