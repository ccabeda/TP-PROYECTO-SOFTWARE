namespace TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Commands
{
    public record LogoutUserCommand
    {
        public int UserId { get; set; }
    }
}
