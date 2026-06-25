namespace TP_PROYECTO_SOFTWARE.API.Configuration;

public static class ConfigurationGuards
{
    public static string GetRequiredJwtKey(this IConfiguration configuration)
    {
        return configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "Jwt:Key no configurado. Antes de levantar la API, definirlo con 'dotnet user-secrets --project backend\\API\\TP-PROYECTO-SOFTWARE.API.csproj set \"Jwt:Key\" \"<clave-de-al-menos-32-caracteres>\"' o con la variable de entorno Jwt__Key.");
    }

    public static string[] GetRequiredCorsOrigins(this IConfiguration configuration)
    {
        var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
        var normalizedOrigins = origins
            .Select(origin => origin.Trim())
            .Where(origin => !string.IsNullOrWhiteSpace(origin))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return normalizedOrigins.Length > 0
            ? normalizedOrigins
            : throw new InvalidOperationException("Cors:AllowedOrigins no configurado. Definir al menos un origen permitido para el frontend.");
    }
}
