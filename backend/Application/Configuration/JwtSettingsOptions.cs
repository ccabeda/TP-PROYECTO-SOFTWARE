namespace TP_PROYECTO_SOFTWARE.Application.Configuration
{
    public class JwtSettingsOptions
    {
        public const string SectionName = "Jwt";

        public int RefreshTokenDays { get; set; }
    }
}
