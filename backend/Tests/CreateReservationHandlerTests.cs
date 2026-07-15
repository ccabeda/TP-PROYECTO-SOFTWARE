using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using TP_PROYECTO_SOFTWARE.Application.Configuration;
using TP_PROYECTO_SOFTWARE.Application.IHandlers;
using TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Application.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Application.Services.Reservations;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Reservations.Commands;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Reservations.Handlers;
using TP_PROYECTO_SOFTWARE.Domain.Constants;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Tests;

public class CreateReservationHandlerTests
{
    [Fact]
    public async Task Handle_WhenSeatIsAvailable_CreatesPendingReservation()
    {
        var fixture = new Fixture();

        var result = await fixture.Handler.Handle(fixture.Command);

        Assert.Equal(ReservationStatuses.Pending, result.Status);
        fixture.ReservationCommand.Verify(x => x.Create(It.Is<Reservation>(r =>
            r.UserId == fixture.User.Id && r.SeatId == fixture.Seat.Id)), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenSeatIsAvailable_MarksSeatAsReservedAndIncrementsVersion()
    {
        var fixture = new Fixture();
        var initialVersion = fixture.Seat.Version;

        await fixture.Handler.Handle(fixture.Command);

        Assert.Equal(SeatStatuses.Reserved, fixture.Seat.Status);
        Assert.Equal(initialVersion + 1, fixture.Seat.Version);
        fixture.SeatCommand.Verify(x => x.Update(fixture.Seat), Times.Once);
    }

    [Fact]
    public async Task Handle_UsesConfiguredExpirationMinutes()
    {
        var fixture = new Fixture(expirationMinutes: 8);

        var result = await fixture.Handler.Handle(fixture.Command);

        Assert.InRange(result.ExpiresAt - result.ReservedAt, TimeSpan.FromMinutes(8), TimeSpan.FromMinutes(8));
    }

    [Fact]
    public async Task Handle_WhenSeatDoesNotExist_ThrowsKeyNotFoundException()
    {
        var fixture = new Fixture();
        fixture.SeatQuery.Setup(x => x.GetById(fixture.Seat.Id)).ReturnsAsync((Seat?)null);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => fixture.Handler.Handle(fixture.Command));

        Assert.Equal("Butaca no encontrada.", exception.Message);
        fixture.UnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenSeatIsAlreadyReserved_RejectsReservation()
    {
        var fixture = new Fixture();
        fixture.Seat.Status = SeatStatuses.Reserved;

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => fixture.Handler.Handle(fixture.Command));

        Assert.Equal("La butaca no se encuentra disponible.", exception.Message);
        fixture.AuditLogHandler.Verify(x => x.Handle(It.IsAny<Application.UseCases.AuditLogs.Commands.CreateAuditLogCommand>()), Times.Once);
        fixture.ReservationCommand.Verify(x => x.Create(It.IsAny<Reservation>()), Times.Never);
    }

    private sealed class Fixture
    {
        public User User { get; } = new() { Id = 7, Name = "Test User", Email = "test@example.com" };
        public Seat Seat { get; } = TestHelpers.CreateSeat(version: 3);
        public CreateReservationCommand Command => new() { CurrentUserId = User.Id, SeatId = Seat.Id };
        public Mock<IRepositorySeatQuery> SeatQuery { get; } = new();
        public Mock<IRepositoryReservationCommand> ReservationCommand { get; } = new();
        public Mock<IRepositorySeatCommand> SeatCommand { get; } = new();
        public Mock<IApplicationUnitOfWork> UnitOfWork { get; } = new();
        public Mock<ICreateAuditLogHandler> AuditLogHandler { get; } = new();
        public CreateReservationHandler Handler { get; }

        public Fixture(int expirationMinutes = 5)
        {
            var reservationQuery = new Mock<IRepositoryReservationQuery>();
            var userQuery = new Mock<IRepositoryUserQuery>();
            var auditLogCommand = new Mock<IRepositoryAuditLogCommand>();
            var expirationService = new Mock<IReservationExpirationService>();
            var mapper = TestHelpers.CreateMapperMock();

            userQuery.Setup(x => x.GetById(User.Id)).ReturnsAsync(User);
            SeatQuery.Setup(x => x.GetById(Seat.Id)).ReturnsAsync(Seat);
            reservationQuery.Setup(x => x.GetActiveBySeatId(Seat.Id)).ReturnsAsync((Reservation?)null);

            Handler = new CreateReservationHandler(
                SeatQuery.Object,
                reservationQuery.Object,
                userQuery.Object,
                ReservationCommand.Object,
                SeatCommand.Object,
                auditLogCommand.Object,
                UnitOfWork.Object,
                AuditLogHandler.Object,
                expirationService.Object,
                Options.Create(new ReservationSettingsOptions { ExpirationMinutes = expirationMinutes }),
                mapper.Object);
        }
    }
}
