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
        /// <returns></returns>
        Task<string> LoginAsync(
            LoginDto login,  
            CancellationToken token);
    }
}
