using EventManager.Domain.Entities.Users;
using EventManager.DTOs.Users;

namespace EventManager.Application.Repositories
{
    public interface IUsersRepository
    {
        Task RegisterAsync(
            RegisterDto register,
            CancellationToken cancellationToken);

        Task<Guid> GetUserIdAsync(
            LoginDto login,
            CancellationToken cancellationToken);

        Task<UserEntity> GetUserAsync(
            LoginDto login, 
            CancellationToken cancellationToken);
    }
}
