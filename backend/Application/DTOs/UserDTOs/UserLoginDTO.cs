namespace TP_PROYECTO_SOFTWARE.Application.DTOs.UserDTOs
{
    public record UserLoginDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
