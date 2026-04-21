namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Commands
{
    public record CreateUserCommand
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
