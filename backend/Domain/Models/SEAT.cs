using System.ComponentModel.DataAnnotations;

namespace TP_PROYECTO_SOFTWARE.Domain.Models;

public class Seat
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public int SectorId { get; set; }
    public string RowIdentifier { get; set; } = string.Empty;
    public int SeatNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public int Version { get; set; }

    public Sector Sector { get; set; } = null!;
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
