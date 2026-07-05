using Shared.Enums;

namespace Users.Application.Contracts.Auth
{
    public record CreateTokenDto(
        string Login, 
        Guid UserId, 
        Roles Role);
}
