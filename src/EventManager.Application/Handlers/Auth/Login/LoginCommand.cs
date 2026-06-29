using EventManager.DTOs.Users;

namespace EventManager.Application.Handlers.Auth.Login
{
    public record LoginCommand(LoginDto LoginDto) : ICommand;
}
