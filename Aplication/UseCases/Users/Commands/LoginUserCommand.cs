namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Commands
{
    public record LoginUserCommand
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
