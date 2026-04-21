using System.ComponentModel.DataAnnotations;

namespace TP_PROYECTO_SOFTWARE.Domain.Models;

public class RESERVATION
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public int UserId { get; set; }
    public Guid SeatId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime ReservedAt { get; set; }
    public DateTime ExpiresAt { get; set; }

    public USER User { get; set; } = null!;
    public SEAT Seat { get; set; } = null!;
}
