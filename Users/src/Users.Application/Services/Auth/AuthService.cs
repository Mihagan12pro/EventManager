using Users.Application.Contracts.Auth;

namespace Users.Application.Services.Auth
{
    internal class AuthService : IAuthService
    {
        public async Task RegisterAsync(
            RegisterDto register,
            CancellationToken token)
        {
            
        }
    }
}
