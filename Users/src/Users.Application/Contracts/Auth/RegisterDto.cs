using Users.Domain.Enums;

namespace Users.Application.Contracts.Auth
{
    public record RegisterDto(
        string Login, 
        string Password,
        Roles Role = Roles.User);
}
