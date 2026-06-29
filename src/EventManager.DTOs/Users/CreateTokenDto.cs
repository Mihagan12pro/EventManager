using EventManager.Domain.Entities.Users.Enums;

namespace EventManager.DTOs.Users
{
    public record CreateTokenDto(
        LoginDto Login, 
        Guid UserId,
        Roles Role);
}
