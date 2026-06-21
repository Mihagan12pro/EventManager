using EventManager.Application.Repositories;
using EventManager.Application.Security;
using EventManager.DTOs.Users;

namespace EventManager.Application.Handlers.Auth.Register
{
    public class RegisterHandler : ICommandHandler<RegisterCommand>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher _passwordHasher;

        public async Task HandleAsync(
            RegisterCommand command,
            CancellationToken cancellationToken)
        {
            RegisterDto register = command.Register with 
            {
                 Password = _passwordHasher.Hash(command.Register.Password)
            };

            await _usersRepository.RegisterAsync(register, cancellationToken);
        }

        public RegisterHandler(IUsersRepository usersRepository, IPasswordHasher passwordHasher)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
        }
    }
}
