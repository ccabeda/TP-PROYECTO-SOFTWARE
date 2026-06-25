using AutoMapper;
using TP_PROYECTO_SOFTWARE.Application.DTOs.AuditLogDTOs;
using TP_PROYECTO_SOFTWARE.Application.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Application.DTOs.PaymentDTOs;
using TP_PROYECTO_SOFTWARE.Application.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Application.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Application.DTOs.SectorDTOs;
using TP_PROYECTO_SOFTWARE.Application.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Reservations.Commands;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Events.Commands;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Seats.Commands;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Sectors.Commands;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Application.Mapping
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<AuditLog, AuditLogGetDTO>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null));
            CreateMap<Event, EventGetDTO>().ReverseMap();
            CreateMap<EventCreateDTO, CreateEventCommand>().ReverseMap();
            CreateMap<Sector, SectorGetDTO>().ReverseMap();
            CreateMap<SectorCreateDTO, CreateSectorCommand>().ReverseMap();
            CreateMap<Seat, SeatGetDTO>().ReverseMap();
            CreateMap<SeatCreateDTO, CreateSeatCommand>().ReverseMap();
            CreateMap<SeatBulkCreateDTO, CreateSeatsBulkCommand>().ReverseMap();
            CreateMap<PaymentCreateDTO, ConfirmReservationPaymentCommand>().ReverseMap();
            CreateMap<ReservationCreateDTO, CreateReservationCommand>().ReverseMap();
            CreateMap<Reservation, ReservationGetDTO>().ReverseMap();
            CreateMap<User, UserGetDTO>().ReverseMap();
            CreateMap<User, UserLoginResponseDTO>().ReverseMap();
            CreateMap<UserCreateDTO, CreateUserCommand>().ReverseMap();
            CreateMap<UserLoginDTO, LoginUserCommand>().ReverseMap();
            CreateMap<RefreshTokenDTO, RefreshUserTokenCommand>().ReverseMap();
        }
    }
}

