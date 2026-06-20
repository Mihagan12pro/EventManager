using EventManager.DTOs.Users;

namespace EventManager.Application.Repositories
{
    public interface IUsersRepository
    {
        Task RegisterAsync(RegisterDto register, CancellationToken cancellationToken);
    }
}
