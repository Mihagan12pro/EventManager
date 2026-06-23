using EventManager.Application.DataAccess.Repositories;
using EventManager.Application.Security;
using EventManager.Domain.Entities.Users;
using EventManager.DTOs.Users;
using Microsoft.Extensions.Configuration;

namespace EventManager.Application.Handlers.Auth.Login
{
    public class LoginHandler : ICommandHandler<string, LoginCommand>
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtWyzard _jwtWyzard;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUsersRepository _usersRepository;

        public async Task<string> HandleAsync(
            LoginCommand command, 
            CancellationToken cancellationToken)
        {
            LoginDto login = command.LoginDto with
            {
                Password = _passwordHasher.Hash(command.LoginDto.Password)
            };
            IConfigurationSection jwtSection = _configuration.GetSection("JwtOptions");

            UserEntity user = await _usersRepository.GetUserAsync(login, cancellationToken);

            CreateTokenDto createTokenDto = new CreateTokenDto(login, user.Id, user.Role);

            string token = _jwtWyzard.Create(createTokenDto, jwtSection);

            return token;
        }

        public LoginHandler(
            IConfiguration configuration,
            IJwtWyzard jwtWyzard,
            IPasswordHasher passwordHasher,
            IUsersRepository usersRepository)
        {
            _configuration = configuration;
            _jwtWyzard = jwtWyzard;
            _passwordHasher = passwordHasher;
            _usersRepository = usersRepository;
        }
    }
}
