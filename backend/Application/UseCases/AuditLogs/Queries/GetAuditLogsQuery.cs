namespace TP_PROYECTO_SOFTWARE.Application.UseCases.AuditLogs.Queries
{
    public record GetAuditLogsQuery
    {
        public int? UserId { get; set; }
        public string? Search { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
