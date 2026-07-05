using Users.Application.Contracts.Auth;
using Users.Domain;

namespace Users.Application.Repositories.Auth
{
    public interface IReadAuthRepository
    {
        Task<User> FindUserAsync(
            LoginDto login, 
            CancellationToken cancellationToken);
    }
}
