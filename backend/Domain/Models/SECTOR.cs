using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP_PROYECTO_SOFTWARE.Domain.Models;

public class SECTOR
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Capacity { get; set; }

    public EVENT Event { get; set; } = null!;
    public ICollection<SEAT> Seats { get; set; } = new List<SEAT>();
}
