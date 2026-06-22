using EventManager.Application.Security;
using EventManager.DTOs.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace EventManager.Infrastructure.Security.Jwt
{
    internal class JwtHmacSha256Wyzard : IJwtWyzard
    {
        public string Create(CreateTokenDto createTokenDto, IConfigurationSection jwtSection)
        {
            var claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString(),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.GetRequiredSection("SecretKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            int minutes = int.Parse(jwtSection.GetRequiredSection("ExpiredMinutes").Value);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, createTokenDto.UserId.ToString()),

                    new Claim(ClaimTypes.Role, createTokenDto.Role.ToString())
                }),
                Issuer = jwtSection.GetRequiredSection("Issuer").Value,
                Audience = jwtSection.GetRequiredSection("Audience").Value,
                Expires = DateTime.UtcNow.AddMinutes(minutes),
                Claims = claims,
                SigningCredentials = creds,
            };

            var tokenString = new JsonWebTokenHandler().CreateToken(descriptor);

            return tokenString;
        }
    }
}
