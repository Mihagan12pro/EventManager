using EventManager.Application.Security;
using EventManager.DTOs.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventManager.Infrastructure.Security
{
    internal class JwtHmacSha256Wyzard : IJwtWyzard
    {
        public string Create(LoginDto login, IConfigurationSection configurationSection)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationSection.GetRequiredSection("SecretKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               issuer: configurationSection.GetRequiredSection("Issuer").Value,
               audience: configurationSection.GetRequiredSection("Audience").Value,
               expires: DateTime.Now.AddMinutes(int.Parse(configurationSection.GetRequiredSection("ExpiredMinutes").Value)),
               signingCredentials: creds
           );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void Foo(string msg = null, Object min = null)
        {

        }
    }
}
