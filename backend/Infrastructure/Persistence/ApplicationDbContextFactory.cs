using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TP_PROYECTO_SOFTWARE.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var apiPath = ResolveApiPath();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiPath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("Connection"));

        return new ApplicationDbContext(optionsBuilder.Options);
    }

    private static string ResolveApiPath()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var candidatePaths = new[]
        {
            Path.Combine(currentDirectory, "backend", "API"),
            Path.Combine(currentDirectory, "..", "API"),
            Path.Combine(currentDirectory, "API"),
        };

        foreach (var candidatePath in candidatePaths)
        {
            var fullPath = Path.GetFullPath(candidatePath);
            var appsettingsPath = Path.Combine(fullPath, "appsettings.json");

            if (Directory.Exists(fullPath) && File.Exists(appsettingsPath))
            {
                return fullPath;
            }
        }

        throw new InvalidOperationException(
            $"No se pudo resolver la carpeta del proyecto API desde '{currentDirectory}'.");
    }
}
