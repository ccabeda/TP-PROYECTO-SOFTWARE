namespace TP_PROYECTO_SOFTWARE.Aplication.DTOs.UserDTOs
{
    public record UserGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
