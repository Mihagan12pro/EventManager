namespace EventManager.Application.Handlers.Auth.Register
{
    public class RegisterHandler : ICommandHandler<RegisterCommand>
    {
        public async Task HandleAsync(
            RegisterCommand command,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
