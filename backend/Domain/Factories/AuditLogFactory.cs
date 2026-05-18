using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Domain.Factories;

public static class AuditLogFactory
{
    public static AuditLog Create(
        int? userId,
        string action,
        string entityType,
        string entityId,
        string details = "")
    {
        return new AuditLog
        {
            UserId = userId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Details = details,
            CreatedAt = DateTime.UtcNow
        };
    }
}
