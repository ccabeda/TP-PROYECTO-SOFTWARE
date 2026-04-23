using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TP_PROYECTO_SOFTWARE.Aplication.ISecurity;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.API.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(USER user)
        {
            var issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer no configurado.");
            var audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience no configurado.");
            var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key no configurado.");
            var role = ResolveRole(user.Email);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.UniqueName, user.Name),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, role)
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string ResolveRole(string email)
        {
            var adminEmails = _configuration.GetSection("AuthorizationSettings:AdminEmails").Get<string[]>() ?? Array.Empty<string>();

            return adminEmails.Contains(email, StringComparer.OrdinalIgnoreCase) ? "Admin" : "User";
        }
    }
}
