using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Handlers
{
    public class CreateReservationHandler
    {
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IRepositoryReservationQuery _repositoryReservationQuery;
        private readonly IRepositoryUserQuery _repositoryUserQuery;
        private readonly IUnitOfWorkReservationCommand _unitOfWorkReservationCommand;
        private readonly IMapper _mapper;

        public CreateReservationHandler(
            IRepositorySeatQuery repositorySeatQuery,
            IRepositoryReservationQuery repositoryReservationQuery,
            IRepositoryUserQuery repositoryUserQuery,
            IUnitOfWorkReservationCommand unitOfWorkReservationCommand,
            IMapper mapper)
        {
            _repositorySeatQuery = repositorySeatQuery;
            _repositoryReservationQuery = repositoryReservationQuery;
            _repositoryUserQuery = repositoryUserQuery;
            _unitOfWorkReservationCommand = unitOfWorkReservationCommand;
            _mapper = mapper;
        }

        public async Task<ReservationGetDTO> Handle(CreateReservationCommand command)
        {
            var user = await _repositoryUserQuery.GetById(command.UserId)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            var seat = await _repositorySeatQuery.GetById(command.SeatId)
                ?? throw new KeyNotFoundException("Butaca no encontrada.");

            var activeReservation = await _repositoryReservationQuery.GetActiveBySeatId(command.SeatId);

            if (seat.Status != "Available" || activeReservation is not null)
            {
                throw new InvalidOperationException("La butaca no se encuentra disponible.");
            }

            seat.Status = "Reserved";
            seat.Version += 1; //version es para que cada vez que se modifique se le sume 1 tengo entendido

            var reservation = new RESERVATION
            {
                UserId = user.Id,
                SeatId = seat.Id,
                Status = "Pending",
                ReservedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };

            var auditLog = new AUDIT_LOG
            {
                UserId = user.Id,
                Action = "CreateReservation",
                EntityType = "RESERVATION",
                EntityId = reservation.Id.ToString(),
                Details = $"Reserva creada para la butaca {seat.Id}",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWorkReservationCommand.RepositorySeatCommand.Update(seat);
            await _unitOfWorkReservationCommand.RepositoryReservationCommand.Create(reservation);
            await _unitOfWorkReservationCommand.RepositoryAuditLogCommand.Create(auditLog);
            await _unitOfWorkReservationCommand.Save();

            return _mapper.Map<ReservationGetDTO>(reservation);
        }
    }
}
