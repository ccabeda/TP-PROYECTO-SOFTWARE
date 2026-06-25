using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Application.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infrastructure.Persistence;
using TP_PROYECTO_SOFTWARE.Infrastructure.Repository.Command;
using TP_PROYECTO_SOFTWARE.Infrastructure.Repository.Query;
using TP_PROYECTO_SOFTWARE.Infrastructure.Transaction;

namespace TP_PROYECTO_SOFTWARE.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection")
            ?? throw new InvalidOperationException(
                "ConnectionStrings:Connection no configurado. Definirlo en appsettings local, User Secrets o variable de entorno ConnectionStrings__Connection.");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services
            .AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
            })
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IRepositoryEventQuery, RepositoryEventQuery>();
        services.AddScoped<IRepositoryAuditLogQuery, RepositoryAuditLogQuery>();
        services.AddScoped<IRepositorySectorQuery, RepositorySectorQuery>();
        services.AddScoped<IRepositorySeatQuery, RepositorySeatQuery>();
        services.AddScoped<IRepositoryReservationQuery, RepositoryReservationQuery>();
        services.AddScoped<IRepositoryUserQuery, RepositoryUserQuery>();
        services.AddScoped<IRepositoryReservationCommand, RepositoryReservationCommand>();
        services.AddScoped<IRepositorySeatCommand, RepositorySeatCommand>();
        services.AddScoped<IRepositoryEventCommand, RepositoryEventCommand>();
        services.AddScoped<IRepositorySectorCommand, RepositorySectorCommand>();
        services.AddScoped<IRepositoryAuditLogCommand, RepositoryAuditLogCommand>();
        services.AddScoped<IRepositoryUserCommand, RepositoryUserCommand>();
        services.AddScoped<IApplicationUnitOfWork, UnitOfWork>();

        return services;
    }
}
