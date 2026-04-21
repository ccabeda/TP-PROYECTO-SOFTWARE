namespace TP_PROYECTO_SOFTWARE.Aplication.DTOs.SectorDTOs
{
    public record SectorCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Capacity { get; set; }
    }
}
