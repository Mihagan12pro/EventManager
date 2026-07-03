using Users.Application.Contracts.Auth;

namespace Users.Application.Repositories.Auth
{
    public interface IWriteAuthRepository
    {
        Task RegisterAsync(
            RegisterDto register,
            CancellationToken cancellationToken);
    }
}
