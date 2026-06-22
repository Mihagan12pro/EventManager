using EventManager.DTOs.Users;
using Microsoft.Extensions.Configuration;

namespace EventManager.Application.Security
{
    public interface IJwtWyzard
    {
        string Create(
            CreateTokenDto createTokenDto, 
            IConfigurationSection configurationSection);
    }
}
