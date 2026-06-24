using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;
using TP_PROYECTO_SOFTWARE.Infraestructure.Repository.Command;
using TP_PROYECTO_SOFTWARE.Infraestructure.Repository.Query;
using TP_PROYECTO_SOFTWARE.Infraestructure.Transaction;

namespace TP_PROYECTO_SOFTWARE.Infraestructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Connection"));
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
            .AddEntityFrameworkStores<AplicationDbContext>()
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
