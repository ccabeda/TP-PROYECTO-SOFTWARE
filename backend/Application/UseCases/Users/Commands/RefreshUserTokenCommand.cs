namespace TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Commands
{
    public record RefreshUserTokenCommand
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
