using Users.Application.Dtos.Auth;

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
