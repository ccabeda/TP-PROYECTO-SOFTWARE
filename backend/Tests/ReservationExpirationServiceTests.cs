using Microsoft.Extensions.Logging;
using Moq;
using TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Application.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Application.Services.Reservations;
using TP_PROYECTO_SOFTWARE.Domain.Constants;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Tests;

public class ReservationExpirationServiceTests
{
    [Fact]
    public async Task ExpirePendingReservations_WhenReservationExpired_MarksItAsExpired()
    {
        var fixture = new Fixture(withExpiredReservation: true);

        await fixture.Service.ExpirePendingReservations();

        Assert.Equal(ReservationStatuses.Expired, fixture.Reservation!.Status);
        fixture.ReservationCommand.Verify(x => x.Update(fixture.Reservation), Times.Once);
    }

    [Fact]
    public async Task ExpirePendingReservations_WhenSeatIsReserved_ReleasesItAndIncrementsVersion()
    {
        var fixture = new Fixture(withExpiredReservation: true);
        var initialVersion = fixture.Seat!.Version;

        await fixture.Service.ExpirePendingReservations();

        Assert.Equal(SeatStatuses.Available, fixture.Seat.Status);
        Assert.Equal(initialVersion + 1, fixture.Seat.Version);
        fixture.SeatCommand.Verify(x => x.Update(fixture.Seat), Times.Once);
    }

    [Fact]
    public async Task ExpirePendingReservations_WhenNoneExpired_DoesNotSaveChanges()
    {
        var fixture = new Fixture(withExpiredReservation: false);

        await fixture.Service.ExpirePendingReservations();

        fixture.UnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
        fixture.ReservationCommand.Verify(x => x.Update(It.IsAny<Reservation>()), Times.Never);
    }

    private sealed class Fixture
    {
        public Seat? Seat { get; }
        public Reservation? Reservation { get; }
        public Mock<IRepositoryReservationCommand> ReservationCommand { get; } = new();
        public Mock<IRepositorySeatCommand> SeatCommand { get; } = new();
        public Mock<IApplicationUnitOfWork> UnitOfWork { get; } = new();
        public ReservationExpirationService Service { get; }

        public Fixture(bool withExpiredReservation)
        {
            var reservationQuery = new Mock<IRepositoryReservationQuery>();
            var auditLogCommand = new Mock<IRepositoryAuditLogCommand>();

            if (withExpiredReservation)
            {
                Seat = TestHelpers.CreateSeat(SeatStatuses.Reserved, version: 4);
                Reservation = new Reservation
                {
                    Id = Guid.NewGuid(),
                    UserId = 7,
                    SeatId = Seat.Id,
                    Seat = Seat,
                    Status = ReservationStatuses.Pending,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(-1)
                };
                reservationQuery.Setup(x => x.GetExpiredPendingReservations(It.IsAny<DateTime>()))
                    .ReturnsAsync(new List<Reservation> { Reservation });
            }
            else
            {
                reservationQuery.Setup(x => x.GetExpiredPendingReservations(It.IsAny<DateTime>()))
                    .ReturnsAsync(new List<Reservation>());
            }

            Service = new ReservationExpirationService(
                reservationQuery.Object,
                ReservationCommand.Object,
                SeatCommand.Object,
                auditLogCommand.Object,
                UnitOfWork.Object,
                Mock.Of<ILogger<ReservationExpirationService>>());
        }
    }
}
