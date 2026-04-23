using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.AuditLogDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.PaymentDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SectorDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.Mapping
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<AUDIT_LOG, AuditLogGetDTO>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null));
            CreateMap<EVENT, EventGetDTO>().ReverseMap();
            CreateMap<EventCreateDTO, CreateEventCommand>().ReverseMap();
            CreateMap<SECTOR, SectorGetDTO>().ReverseMap();
            CreateMap<SectorCreateDTO, CreateSectorCommand>().ReverseMap();
            CreateMap<SEAT, SeatGetDTO>().ReverseMap();
            CreateMap<SeatCreateDTO, CreateSeatCommand>().ReverseMap();
            CreateMap<SeatBulkCreateDTO, CreateSeatsBulkCommand>().ReverseMap();
            CreateMap<PaymentCreateDTO, ConfirmReservationPaymentCommand>().ReverseMap();
            CreateMap<ReservationCreateDTO, CreateReservationCommand>().ReverseMap();
            CreateMap<RESERVATION, ReservationGetDTO>().ReverseMap();
            CreateMap<USER, UserGetDTO>().ReverseMap();
            CreateMap<USER, UserLoginResponseDTO>().ReverseMap();
            CreateMap<UserCreateDTO, CreateUserCommand>().ReverseMap();
            CreateMap<UserLoginDTO, LoginUserCommand>().ReverseMap();
        }
    }
}
