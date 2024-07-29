using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TicketsApi.Models;

namespace TicketsApi.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private static readonly ConcurrentDictionary<string, DateTime> RevokedTokens = new();

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]) ?? throw new InvalidOperationException("JWT Key não configurada");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role , user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public void RevokeToken(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var expiryDate = jwtToken.ValidTo;
            RevokedTokens[token] = expiryDate;
        }

        public bool IsTokenRevoked(string token)
        {
            if (RevokedTokens.TryGetValue(token, out var expiryDate))
            {
                return DateTime.UtcNow < expiryDate;
            }
            return false;
        }
    }
}
