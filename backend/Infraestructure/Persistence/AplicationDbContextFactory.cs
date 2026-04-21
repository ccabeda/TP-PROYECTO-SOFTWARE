using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

public class AplicationDbContextFactory : IDesignTimeDbContextFactory<AplicationDbContext>
{
    public AplicationDbContext CreateDbContext(string[] args)
    {
        var apiPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "API"));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiPath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AplicationDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("Connection"));

        return new AplicationDbContext(optionsBuilder.Options);
    }
}
