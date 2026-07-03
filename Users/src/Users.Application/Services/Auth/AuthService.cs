using Users.Application.Contracts.Auth;
using Users.Application.Repositories.Auth;
using Users.Application.Security;

namespace Users.Application.Services.Auth
{
    internal class AuthService : IAuthService
    {
        private readonly IWriteAuthRepository _writeAuthRepository;
        private readonly IReadAuthRepository _readAuthRepository;
        private readonly IPasswordHasher _passwordHasher;

        public async Task RegisterAsync(
            RegisterDto register,
            CancellationToken cancellationToken)
        {
            register = register with
            {
                Password = _passwordHasher.Hash(register.Password),

                Login = register.Login.ToLower()
            };

            await _writeAuthRepository.RegisterAsync(register, cancellationToken);
        }

        public AuthService(
            IWriteAuthRepository writeAuthRepository,
            IReadAuthRepository readAuthRepository,
            IPasswordHasher passwordHasher)
        {
            _readAuthRepository = readAuthRepository;
            _writeAuthRepository = writeAuthRepository;
            _passwordHasher = passwordHasher;
        }
    }
}
