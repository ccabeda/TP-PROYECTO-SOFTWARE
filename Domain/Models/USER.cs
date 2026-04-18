using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP_PROYECTO_SOFTWARE.Domain.Models;

public class USER
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public ICollection<RESERVATION> Reservations { get; set; } = new List<RESERVATION>();
    public ICollection<AUDIT_LOG> AuditLogs { get; set; } = new List<AUDIT_LOG>();
}
