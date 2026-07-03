using Users.Application.Contracts.Auth;
using Users.Application.Repositories.Auth;

namespace Users.Application.Services.Auth
{
    internal class AuthService : IAuthService
    {
        private readonly IWriteAuthRepository _writeAuthRepository;
        private readonly IReadAuthRepository _readAuthRepository;

        public async Task RegisterAsync(
            RegisterDto register,
            CancellationToken cancellationToken)
        {
            await _writeAuthRepository.RegisterAsync(register, cancellationToken);
        }

        public AuthService(
            IWriteAuthRepository writeAuthRepository,
            IReadAuthRepository readAuthRepository)
        {
            _readAuthRepository = readAuthRepository;
            _writeAuthRepository = writeAuthRepository;
        }
    }
}
