using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TP_PROYECTO_SOFTWARE.Application.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Application.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Domain.Constants;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Tests;

internal static class TestHelpers
{
    public static Seat CreateSeat(string status = SeatStatuses.Available, int version = 0)
    {
        return new Seat
        {
            Id = Guid.NewGuid(),
            Status = status,
            Version = version,
            Sector = new Sector
            {
                Event = new TP_PROYECTO_SOFTWARE.Domain.Models.Event { EventDate = DateTime.UtcNow.AddDays(1) }
            }
        };
    }

    public static Mock<IMapper> CreateMapperMock()
    {
        var mapper = new Mock<IMapper>();
        mapper.Setup(x => x.Map<ReservationGetDTO>(It.IsAny<object>()))
            .Returns((object source) =>
            {
                var reservation = (Reservation)source;
                return new ReservationGetDTO
                {
                    Id = reservation.Id,
                    UserId = reservation.UserId,
                    SeatId = reservation.SeatId,
                    Status = reservation.Status,
                    ReservedAt = reservation.ReservedAt,
                    ExpiresAt = reservation.ExpiresAt
                };
            });
        mapper.Setup(x => x.Map<UserLoginResponseDTO>(It.IsAny<object>()))
            .Returns((object source) =>
            {
                var user = (User)source;
                return new UserLoginResponseDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email ?? string.Empty
                };
            });
        return mapper;
    }

    public static Mock<UserManager<User>> CreateUserManagerMock()
    {
        var store = new Mock<IUserStore<User>>();
        return new Mock<UserManager<User>>(
            store.Object,
            Options.Create(new IdentityOptions()),
            new PasswordHasher<User>(),
            Array.Empty<IUserValidator<User>>(),
            Array.Empty<IPasswordValidator<User>>(),
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            null!,
            Mock.Of<ILogger<UserManager<User>>>());
    }
}
