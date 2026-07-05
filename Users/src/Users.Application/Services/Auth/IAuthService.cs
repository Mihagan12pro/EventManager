using Users.Application.Dtos.Auth;

namespace Users.Application.Services.Auth
{
    public interface IAuthService
    {
        Task RegisterAsync(
            RegisterDto register, 
            CancellationToken token);

        /// <summary>
        /// Logins users
        /// </summary>
        /// <param name="login"></param>
        /// <param name="token"></param>
        /// <returns>Token and user id</returns>
        Task<(string, Guid)> LoginAsync(
            LoginDto login,  
            CancellationToken token);
    }
}
