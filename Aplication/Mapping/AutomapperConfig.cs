using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SectorDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.Mapping
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<EVENT, EventGetDTO>().ReverseMap();
            CreateMap<SECTOR, SectorGetDTO>().ReverseMap();
            CreateMap<SEAT, SeatGetDTO>().ReverseMap();
            CreateMap<ReservationCreateDTO, CreateReservationCommand>().ReverseMap();
            CreateMap<RESERVATION, ReservationGetDTO>().ReverseMap();
        }
    }
}
