using Users.Application.Dtos.Auth;
using Users.Application.Repositories.Auth;
using Users.Application.Security;
using Users.Application.Security.Jwt;
using Users.Domain;

namespace Users.Application.Services.Auth
{
    internal class AuthService : IAuthService
    {
        private readonly IWriteAuthRepository _writeAuthRepository;
        private readonly IReadAuthRepository _readAuthRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtWizard _jwtWizard;

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

        public async Task<string> LoginAsync(
            LoginDto login, 
            CancellationToken cancellationToken)
        {
            login = login with
            {
                Password = _passwordHasher.Hash(login.Password)
            };

            User user = await _readAuthRepository.FindUserAsync(login, cancellationToken);

            string token = _jwtWizard.Create(new CreateTokenDto(login.Login, user.Id, user.Role));

            return token;
        }

        public AuthService(
            IWriteAuthRepository writeAuthRepository,
            IReadAuthRepository readAuthRepository,
            IPasswordHasher passwordHasher,
            IJwtWizard jwtWizard)
        {
            _readAuthRepository = readAuthRepository;
            _writeAuthRepository = writeAuthRepository;
            _passwordHasher = passwordHasher;
            _jwtWizard = jwtWizard;
        }
    }
}
