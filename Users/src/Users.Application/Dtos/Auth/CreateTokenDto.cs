using Shared.Enums;

namespace Users.Application.Dtos.Auth
{
    public record CreateTokenDto(
        string Login, 
        Guid UserId, 
        Roles Role);
}
