namespace TP_PROYECTO_SOFTWARE.Aplication.DTOs.AuditLogDTOs
{
    public record AuditLogGetDTO
    {
        public Guid Id { get; set; }
        public int? UserId { get; set; }
        public string? UserEmail { get; set; }
        public string Action { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
