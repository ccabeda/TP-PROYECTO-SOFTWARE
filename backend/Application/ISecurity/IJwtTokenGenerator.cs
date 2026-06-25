using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Application.ISecurity
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, IEnumerable<string> roles);
        string GenerateRefreshToken();
        string HashRefreshToken(string refreshToken);
        string ResolvePrimaryRole(IEnumerable<string> roles);
    }
}

