using Moq;
using TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Application.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Application.Services.Reservations;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Reservations.Commands;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Reservations.Handlers;
using TP_PROYECTO_SOFTWARE.Domain.Constants;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Tests;

public class ConfirmReservationPaymentHandlerTests
{
    [Fact]
    public async Task Handle_WhenReservationIsValid_ConfirmsPaymentAtomically()
    {
        var fixture = new Fixture();

        var result = await fixture.Handler.Handle(fixture.Command);

        Assert.Equal(ReservationStatuses.Paid, result.Status);
        Assert.Equal(SeatStatuses.Sold, fixture.Seat.Status);
        fixture.UnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
        fixture.UnitOfWork.Verify(x => x.CommitTransactionAsync(), Times.Once);
        fixture.UnitOfWork.Verify(x => x.RollbackTransactionAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenReservationIsAlreadyPaid_RejectsPayment()
    {
        var fixture = new Fixture();
        fixture.Reservation.Status = ReservationStatuses.Paid;

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => fixture.Handler.Handle(fixture.Command));

        Assert.Equal("La reserva no se encuentra pendiente de pago.", exception.Message);
        fixture.UnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenReservationExpired_ReleasesSeatAndRejectsPayment()
    {
        var fixture = new Fixture();
        fixture.Reservation.ExpiresAt = DateTime.UtcNow.AddMinutes(-1);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => fixture.Handler.Handle(fixture.Command));

        Assert.Equal("La reserva expiró. Selecciona otra butaca.", exception.Message);
        Assert.Equal(ReservationStatuses.Expired, fixture.Reservation.Status);
        Assert.Equal(SeatStatuses.Available, fixture.Seat.Status);
    }

    private sealed class Fixture
    {
        public Seat Seat { get; } = TestHelpers.CreateSeat(SeatStatuses.Reserved, version: 2);
        public Reservation Reservation { get; }
        public ConfirmReservationPaymentCommand Command { get; }
        public Mock<IApplicationUnitOfWork> UnitOfWork { get; } = new();
        public ConfirmReservationPaymentHandler Handler { get; }

        public Fixture()
        {
            Reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                UserId = 7,
                SeatId = Seat.Id,
                Seat = Seat,
                Status = ReservationStatuses.Pending,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };
            Command = new ConfirmReservationPaymentCommand
            {
                ReservationId = Reservation.Id,
                CurrentUserId = Reservation.UserId
            };

            var reservationQuery = new Mock<IRepositoryReservationQuery>();
            var seatQuery = new Mock<IRepositorySeatQuery>();
            var reservationCommand = new Mock<IRepositoryReservationCommand>();
            var seatCommand = new Mock<IRepositorySeatCommand>();
            var auditLogCommand = new Mock<IRepositoryAuditLogCommand>();
            var expirationService = new Mock<IReservationExpirationService>();

            reservationQuery.Setup(x => x.GetById(Reservation.Id)).ReturnsAsync(Reservation);
            seatQuery.Setup(x => x.GetById(Seat.Id)).ReturnsAsync(Seat);

            Handler = new ConfirmReservationPaymentHandler(
                reservationQuery.Object,
                seatQuery.Object,
                reservationCommand.Object,
                seatCommand.Object,
                auditLogCommand.Object,
                UnitOfWork.Object,
                expirationService.Object,
                TestHelpers.CreateMapperMock().Object);
        }
    }
}
