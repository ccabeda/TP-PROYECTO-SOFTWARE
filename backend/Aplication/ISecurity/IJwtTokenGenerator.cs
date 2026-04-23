using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.ISecurity
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(USER user);
        string ResolveRole(string email);
    }
}
