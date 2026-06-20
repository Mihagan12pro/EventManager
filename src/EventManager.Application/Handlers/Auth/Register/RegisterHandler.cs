using EventManager.Application.Security;
using EventManager.DTOs.Users;

namespace EventManager.Application.Handlers.Auth.Register
{
    public class RegisterHandler : ICommandHandler<RegisterCommand>
    {
        private readonly IPasswordHasher _passwordHasher;

        public async Task HandleAsync(
            RegisterCommand command,
            CancellationToken cancellationToken)
        {
            RegisterDto register = command.Register with 
            {
                 Password = _passwordHasher.Hash(command.Register.Password)
            };

            throw new NotImplementedException();
        }

        public RegisterHandler(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }
    }
}
