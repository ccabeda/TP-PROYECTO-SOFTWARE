using System.ComponentModel.DataAnnotations;

namespace TP_PROYECTO_SOFTWARE.Domain.Models;

public class SEAT
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public int SectorId { get; set; }
    public string RowIdentifier { get; set; } = string.Empty;
    public int SeatNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public int Version { get; set; }

    public SECTOR Sector { get; set; } = null!;
    public ICollection<RESERVATION> Reservations { get; set; } = new List<RESERVATION>();
}
