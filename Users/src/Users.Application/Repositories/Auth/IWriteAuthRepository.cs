using Users.Application.Dtos.Auth;

namespace Users.Application.Repositories.Auth
{
    public interface IWriteAuthRepository
    {
        Task RegisterAsync(
            RegisterDto register,
            CancellationToken cancellationToken);
    }
}
