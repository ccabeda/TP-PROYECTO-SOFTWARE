using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Commands;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Handlers
{
    public class ConfirmReservationPaymentHandler : IConfirmReservationPaymentHandler
    {
        private readonly IRepositoryReservationQuery _repositoryReservationQuery;
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IUnitOfWorkReservationCommand _unitOfWorkReservationCommand;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IMapper _mapper;

        public ConfirmReservationPaymentHandler(
            IRepositoryReservationQuery repositoryReservationQuery,
            IRepositorySeatQuery repositorySeatQuery,
            IUnitOfWorkReservationCommand unitOfWorkReservationCommand,
            ICreateAuditLogHandler createAuditLogHandler,
            IMapper mapper)
        {
            _repositoryReservationQuery = repositoryReservationQuery;
            _repositorySeatQuery = repositorySeatQuery;
            _unitOfWorkReservationCommand = unitOfWorkReservationCommand;
            _createAuditLogHandler = createAuditLogHandler;
            _mapper = mapper;
        }

        public async Task<ReservationGetDTO> Handle(ConfirmReservationPaymentCommand command)
        {
            var reservation = await _repositoryReservationQuery.GetById(command.ReservationId)
                ?? throw new KeyNotFoundException("Reserva no encontrada.");

            if (reservation.Status != "Pending")
            {
                throw new InvalidOperationException("La reserva no se encuentra pendiente de pago.");
            }

            var seat = await _repositorySeatQuery.GetById(reservation.SeatId)
                ?? throw new KeyNotFoundException("Butaca no encontrada.");

            if (seat.Status != "Reserved")
            {
                throw new InvalidOperationException("La butaca no se encuentra reservada.");
            }

            reservation.Status = "Paid";
            seat.Status = "Sold";
            seat.Version += 1;

            await _unitOfWorkReservationCommand.RepositoryReservationCommand.Update(reservation);
            await _unitOfWorkReservationCommand.RepositorySeatCommand.Update(seat);
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = reservation.UserId,
                Action = "ConfirmReservationPayment",
                EntityType = "RESERVATION",
                EntityId = reservation.Id.ToString(),
                Details = $"Pago confirmado. ReservationId={reservation.Id}, SeatId={seat.Id}, UserId={reservation.UserId}, ReservationStatus={reservation.Status}, SeatStatus={seat.Status}"
            });
            await _unitOfWorkReservationCommand.Save();

            return _mapper.Map<ReservationGetDTO>(reservation);
        }
    }
}
