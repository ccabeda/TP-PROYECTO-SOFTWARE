using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using TP_PROYECTO_SOFTWARE.Application.Configuration;
using TP_PROYECTO_SOFTWARE.Application.IHandlers;
using TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Application.ISecurity;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Commands;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Handlers;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Tests;

public class AuthenticationHandlerTests
{
    [Fact]
    public async Task Login_WhenCredentialsAreValid_ReturnsTokensAndStoresRefreshHash()
    {
        var user = CreateUser();
        var userManager = TestHelpers.CreateUserManagerMock();
        var tokenGenerator = CreateTokenGeneratorMock();
        var auditHandler = new Mock<ICreateAuditLogHandler>();
        userManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        userManager.Setup(x => x.CheckPasswordAsync(user, "correct-password")).ReturnsAsync(true);
        userManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
        userManager.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
        var handler = new LoginUserHandler(
            userManager.Object,
            tokenGenerator.Object,
            auditHandler.Object,
            Options.Create(new JwtSettingsOptions { RefreshTokenDays = 7 }),
            TestHelpers.CreateMapperMock().Object);

        var result = await handler.Handle(new LoginUserCommand
        {
            Email = user.Email!,
            Password = "correct-password"
        });

        Assert.Equal("access-token", result.Token);
        Assert.Equal("new-refresh-token", result.RefreshToken);
        Assert.Equal("hashed-refresh-token", user.RefreshTokenHash);
        Assert.InRange(user.RefreshTokenExpiresAt!.Value, DateTime.UtcNow.AddDays(7).AddSeconds(-2), DateTime.UtcNow.AddDays(7).AddSeconds(2));
    }

    [Fact]
    public async Task Login_WhenPasswordIsInvalid_ThrowsUnauthorizedAccessException()
    {
        var user = CreateUser();
        var userManager = TestHelpers.CreateUserManagerMock();
        var auditHandler = new Mock<ICreateAuditLogHandler>();
        userManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        userManager.Setup(x => x.CheckPasswordAsync(user, "wrong-password")).ReturnsAsync(false);
        var handler = new LoginUserHandler(
            userManager.Object,
            CreateTokenGeneratorMock().Object,
            auditHandler.Object,
            Options.Create(new JwtSettingsOptions { RefreshTokenDays = 7 }),
            TestHelpers.CreateMapperMock().Object);

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(new LoginUserCommand
        {
            Email = user.Email!,
            Password = "wrong-password"
        }));

        Assert.Contains("Email o contraseña incorrectos", exception.Message);
        userManager.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task RefreshToken_WhenValid_RotatesRefreshTokenAndReturnsNewAccessToken()
    {
        var user = CreateUser();
        user.RefreshTokenHash = "old-hash";
        var userManager = TestHelpers.CreateUserManagerMock();
        var userQuery = new Mock<IRepositoryUserQuery>();
        var tokenGenerator = CreateTokenGeneratorMock();
        tokenGenerator.Setup(x => x.HashRefreshToken("old-refresh-token")).Returns("old-hash");
        userQuery.Setup(x => x.GetByValidRefreshTokenHash("old-hash", It.IsAny<DateTime>())).ReturnsAsync(user);
        userManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
        userManager.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
        var handler = new RefreshUserTokenHandler(
            userManager.Object,
            userQuery.Object,
            tokenGenerator.Object,
            Options.Create(new JwtSettingsOptions { RefreshTokenDays = 7 }),
            TestHelpers.CreateMapperMock().Object);

        var result = await handler.Handle(new RefreshUserTokenCommand { RefreshToken = "old-refresh-token" });

        Assert.Equal("access-token", result.Token);
        Assert.Equal("new-refresh-token", result.RefreshToken);
        Assert.Equal("hashed-refresh-token", user.RefreshTokenHash);
        userManager.Verify(x => x.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task Logout_WhenUserExists_ClearsStoredRefreshToken()
    {
        var user = CreateUser();
        user.RefreshTokenHash = "stored-hash";
        user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
        var userManager = TestHelpers.CreateUserManagerMock();
        userManager.Setup(x => x.FindByIdAsync(user.Id.ToString())).ReturnsAsync(user);
        userManager.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
        var handler = new LogoutUserHandler(userManager.Object);

        await handler.Handle(new LogoutUserCommand { UserId = user.Id });

        Assert.Null(user.RefreshTokenHash);
        Assert.Null(user.RefreshTokenExpiresAt);
        userManager.Verify(x => x.UpdateAsync(user), Times.Once);
    }

    private static User CreateUser() => new()
    {
        Id = 11,
        Name = "Test User",
        Email = "test@example.com"
    };

    private static Mock<IJwtTokenGenerator> CreateTokenGeneratorMock()
    {
        var tokenGenerator = new Mock<IJwtTokenGenerator>();
        tokenGenerator.Setup(x => x.GenerateToken(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
            .Returns("access-token");
        tokenGenerator.Setup(x => x.GenerateRefreshToken()).Returns("new-refresh-token");
        tokenGenerator.Setup(x => x.HashRefreshToken("new-refresh-token")).Returns("hashed-refresh-token");
        tokenGenerator.Setup(x => x.ResolvePrimaryRole(It.IsAny<IEnumerable<string>>())).Returns("User");
        return tokenGenerator;
    }
}
