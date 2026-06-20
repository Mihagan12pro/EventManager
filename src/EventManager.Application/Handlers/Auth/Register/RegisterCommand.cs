using EventManager.Domain.Entities.Users.Enums;

namespace EventManager.Application.Handlers.Auth.Register
{
    public record RegisterCommand(
        string Login, 
        string Password,
        Roles Role) : ICommand;
}
