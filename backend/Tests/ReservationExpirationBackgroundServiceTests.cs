using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TP_PROYECTO_SOFTWARE.API.BackgroundServices;
using TP_PROYECTO_SOFTWARE.Application.Services.Reservations;

namespace TP_PROYECTO_SOFTWARE.Tests;

public class ReservationExpirationBackgroundServiceTests
{
    [Fact]
    public async Task ExecuteAsync_CreatesScopeAndRunsExpirationService()
    {
        using var cancellation = new CancellationTokenSource();
        var expirationService = new Mock<IReservationExpirationService>();
        expirationService
            .Setup(x => x.ExpirePendingReservations())
            .Callback(cancellation.Cancel)
            .Returns(Task.CompletedTask);

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider
            .Setup(x => x.GetService(typeof(IReservationExpirationService)))
            .Returns(expirationService.Object);

        var scope = new Mock<IServiceScope>();
        scope.SetupGet(x => x.ServiceProvider).Returns(serviceProvider.Object);

        var scopeFactory = new Mock<IServiceScopeFactory>();
        scopeFactory.Setup(x => x.CreateScope()).Returns(scope.Object);

        var backgroundService = new TestableReservationExpirationBackgroundService(
            scopeFactory.Object,
            Mock.Of<ILogger<ReservationExpirationBackgroundService>>());

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => backgroundService.RunAsync(cancellation.Token));

        scopeFactory.Verify(x => x.CreateScope(), Times.Once);
        expirationService.Verify(x => x.ExpirePendingReservations(), Times.Once);
        scope.Verify(x => x.Dispose(), Times.Once);
    }

    private sealed class TestableReservationExpirationBackgroundService : ReservationExpirationBackgroundService
    {
        public TestableReservationExpirationBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<ReservationExpirationBackgroundService> logger)
            : base(scopeFactory, logger)
        {
        }

        public Task RunAsync(CancellationToken cancellationToken) => ExecuteAsync(cancellationToken);
    }
}
