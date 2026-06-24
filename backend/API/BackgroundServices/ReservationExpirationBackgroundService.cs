using TP_PROYECTO_SOFTWARE.Aplication.Services.Reservations;

namespace TP_PROYECTO_SOFTWARE.API.BackgroundServices;

public class ReservationExpirationBackgroundService : BackgroundService
{
    private static readonly TimeSpan ExecutionInterval = TimeSpan.FromMinutes(1);
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ReservationExpirationBackgroundService> _logger;

    public ReservationExpirationBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<ReservationExpirationBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ExpirePendingReservations(stoppingToken);
            await Task.Delay(ExecutionInterval, stoppingToken);
        }
    }

    private async Task ExpirePendingReservations(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var expirationService = scope.ServiceProvider.GetRequiredService<IReservationExpirationService>();

            await expirationService.ExpirePendingReservations();
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al expirar reservas pendientes en segundo plano.");
        }
    }
}
