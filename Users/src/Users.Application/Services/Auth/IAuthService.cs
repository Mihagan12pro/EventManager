using Users.Application.Contracts.Auth;

namespace Users.Application.Services.Auth
{
    public interface IAuthService
    {
        Task RegisterAsync(
            RegisterDto register, 
            CancellationToken token);

        Task<string> LoginAsync(
            LoginDto login,  
            CancellationToken token);
    }
}
