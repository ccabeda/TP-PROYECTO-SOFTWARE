namespace TP_PROYECTO_SOFTWARE.Application.Configuration
{
    public class AuthorizationSettingsOptions
    {
        public const string SectionName = "AuthorizationSettings";

        public List<string> AdminEmails { get; set; } = new();
    }
}
