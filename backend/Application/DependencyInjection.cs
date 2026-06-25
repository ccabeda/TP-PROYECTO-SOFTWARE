using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TP_PROYECTO_SOFTWARE.Application.Configuration;
using TP_PROYECTO_SOFTWARE.Application.IHandlers;
using TP_PROYECTO_SOFTWARE.Application.Mapping;
using TP_PROYECTO_SOFTWARE.Application.Services.Reservations;
using TP_PROYECTO_SOFTWARE.Application.Services.Seats;
using TP_PROYECTO_SOFTWARE.Application.UseCases.AuditLogs.Handlers;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Events.Handlers;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Reservations.Handlers;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Seats.Handlers;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Sectors.Handlers;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Handlers;
using TP_PROYECTO_SOFTWARE.Application.Validations.Event;
using TP_PROYECTO_SOFTWARE.Application.Validations.Seat;
using TP_PROYECTO_SOFTWARE.Application.Validations.Sector;
using TP_PROYECTO_SOFTWARE.Application.Validations.User;

namespace TP_PROYECTO_SOFTWARE.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TicketingRulesOptions>()
            .Bind(configuration.GetSection(TicketingRulesOptions.SectionName))
            .Validate(options => options.MaxSectorsPerEvent > 0, "MaxSectorsPerEvent debe ser mayor a 0.")
            .Validate(options => options.MaxSectorCapacity > 0, "MaxSectorCapacity debe ser mayor a 0.")
            .Validate(options => options.MaxRowsPerBulkCreate > 0, "MaxRowsPerBulkCreate debe ser mayor a 0.")
            .Validate(options => options.MaxSeatsPerRow > 0, "MaxSeatsPerRow debe ser mayor a 0.")
            .Validate(options => options.RowLabels.Count >= options.MaxRowsPerBulkCreate,
                "RowLabels debe tener al menos la misma cantidad de filas que MaxRowsPerBulkCreate.")
            .Validate(options => options.RowLabels
                .Take(options.MaxRowsPerBulkCreate)
                .Select(row => row.Trim().ToUpperInvariant())
                .Where(row => !string.IsNullOrWhiteSpace(row))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count() == options.MaxRowsPerBulkCreate,
                "Las filas configuradas en RowLabels deben ser únicas y no vacías.")
            .ValidateOnStart();

        services.AddOptions<AuthorizationSettingsOptions>()
            .Bind(configuration.GetSection(AuthorizationSettingsOptions.SectionName))
            .ValidateOnStart();

        services.AddOptions<JwtSettingsOptions>()
            .Bind(configuration.GetSection(JwtSettingsOptions.SectionName))
            .Validate(options => options.RefreshTokenDays > 0, "RefreshTokenDays debe ser mayor a 0.")
            .ValidateOnStart();

        services.AddOptions<ReservationSettingsOptions>()
            .Bind(configuration.GetSection(ReservationSettingsOptions.SectionName))
            .Validate(options => options.ExpirationMinutes > 0, "ExpirationMinutes debe ser mayor a 0.")
            .ValidateOnStart();

        services.AddAutoMapper(_ => { }, typeof(AutomapperConfig).Assembly);
        services.AddScoped<ISeatRulesService, SeatRulesService>();
        services.AddScoped<IReservationExpirationService, ReservationExpirationService>();
        services.AddScoped<IGetEventsHandler, GetEventsHandler>();
        services.AddScoped<IGetAuditLogsHandler, GetAuditLogsHandler>();
        services.AddScoped<IGetEventByIdHandler, GetEventByIdHandler>();
        services.AddScoped<ICreateEventHandler, CreateEventHandler>();
        services.AddScoped<IDeleteEventHandler, DeleteEventHandler>();
        services.AddScoped<IGetSectorsByEventHandler, GetSectorsByEventHandler>();
        services.AddScoped<IGetSectorByIdHandler, GetSectorByIdHandler>();
        services.AddScoped<ICreateSectorHandler, CreateSectorHandler>();
        services.AddScoped<IDeleteSectorHandler, DeleteSectorHandler>();
        services.AddScoped<IGetSeatsByEventHandler, GetSeatsByEventHandler>();
        services.AddScoped<IGetSeatsBySectorHandler, GetSeatsBySectorHandler>();
        services.AddScoped<IGetSeatByIdHandler, GetSeatByIdHandler>();
        services.AddScoped<ICreateSeatHandler, CreateSeatHandler>();
        services.AddScoped<ICreateSeatsBulkHandler, CreateSeatsBulkHandler>();
        services.AddScoped<IDeleteSeatHandler, DeleteSeatHandler>();
        services.AddScoped<IGetUsersHandler, GetUsersHandler>();
        services.AddScoped<IGetCurrentUserHandler, GetCurrentUserHandler>();
        services.AddScoped<IGetUserByIdHandler, GetUserByIdHandler>();
        services.AddScoped<ICreateUserHandler, CreateUserHandler>();
        services.AddScoped<ILoginUserHandler, LoginUserHandler>();
        services.AddScoped<IRefreshUserTokenHandler, RefreshUserTokenHandler>();
        services.AddScoped<ILogoutUserHandler, LogoutUserHandler>();
        services.AddScoped<ICreateAuditLogHandler, CreateAuditLogHandler>();
        services.AddScoped<ICreateReservationHandler, CreateReservationHandler>();
        services.AddScoped<IConfirmReservationPaymentHandler, ConfirmReservationPaymentHandler>();
        services.AddScoped<IGetReservationByIdHandler, GetReservationByIdHandler>();
        services.AddScoped<IGetMyReservationsHandler, GetMyReservationsHandler>();

        services.AddValidatorsFromAssemblyContaining<UserCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<EventCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<SectorCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<SeatCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<SeatBulkCreateValidator>();

        return services;
    }
}
