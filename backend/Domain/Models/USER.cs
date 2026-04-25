using Microsoft.AspNetCore.Identity;

namespace TP_PROYECTO_SOFTWARE.Domain.Models;

public class User : IdentityUser<int> //identity visto en clase
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}

