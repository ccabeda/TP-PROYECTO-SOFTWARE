using System.Security.Claims;

namespace TP_PROYECTO_SOFTWARE.API.Helpers
{
    public static class UserClaimsHelper
    {
        public static int? GetCurrentUserId(ClaimsPrincipal user)
        {
            var claimValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(claimValue, out var userId) ? userId : null;
        }
    }
}
