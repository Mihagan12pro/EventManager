using EventManager.Domain.Entities.Users.Enums;
using EventManager.DTOs.Users;

namespace EventManager.Application.Handlers.Auth.Register
{
    public record RegisterCommand(RegisterDto Register) : ICommand;
}
