using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Shared.Objects.Classes;
using System.Security.Claims;
using System.Text;
using Users.Application.Contracts.Auth;
using Users.Application.Security.Jwt;

namespace Users.Infrastructure.Security.Jwt
{
    internal class JwtHmacSha256Wyzard : IJwtWizard
    {
        private readonly AuthOptions _authOptions;

        public string Create(CreateTokenDto createTokenDto)
        {
            var claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString(),

                [JwtRegisteredClaimNames.Aud] = _authOptions.Audiences
            };

            var key = new SymmetricSecurityKey(_authOptions.IssuerSigningKey);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            int minutes = int.Parse(_authOptions.ExpiredMinutes);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, createTokenDto.UserId.ToString()),

                    new Claim("role", createTokenDto.Role.ToString())
                }),

                Issuer = _authOptions.Issuer,
                Expires = DateTime.UtcNow.AddMinutes(minutes),
                Claims = claims,
                SigningCredentials = creds,
            };

            var tokenString = new JsonWebTokenHandler().CreateToken(descriptor);

            return tokenString;
        }

        public JwtHmacSha256Wyzard()
        {
            _authOptions = new AuthOptions();
        }
    }
}
