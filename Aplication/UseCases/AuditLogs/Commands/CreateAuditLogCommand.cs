namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands
{
    public record CreateAuditLogCommand
    {
        public int? UserId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }
}
