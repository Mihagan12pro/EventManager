using EventManager.Application.Security;
using Microsoft.Extensions.Configuration;

namespace EventManager.Application.Handlers.Auth.Login
{
    public class LoginHandler : ICommandHandler<string, LoginCommand>
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtWyzard _jwtWyzard;

        public async Task<string> HandleAsync(
            LoginCommand command, 
            CancellationToken cancellationToken)
        {
            IConfigurationSection configurationSection = _configuration.GetSection("JwtOptions");
            string token = _jwtWyzard.Create(command.LoginDto, configurationSection);

            return token;
        }

        public LoginHandler(IConfiguration configuration, IJwtWyzard jwtWyzard)
        {
            _configuration = configuration;
            _jwtWyzard = jwtWyzard;
        }
    }
}
