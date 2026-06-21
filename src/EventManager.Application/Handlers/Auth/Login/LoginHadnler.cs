using Microsoft.Extensions.Configuration;

namespace EventManager.Application.Handlers.Auth.Login
{
    public class LoginHandler : ICommandHandler<string, LoginCommand>
    {
        private readonly IConfiguration _configuration;

        public async Task<string> HandleAsync(
            LoginCommand command, 
            CancellationToken cancellationToken)
        {
            return string.Empty;
        }

        public LoginHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
