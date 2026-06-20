namespace EventManager.Application.Handlers
{
    public interface ICommandHandler<T, TCommand> 
        where TCommand : ICommand
    {
        Task<T> HandleAsync(
            TCommand command,
            CancellationToken cancellationToken);
    }

    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task HandleAsync(
            TCommand command, 
            CancellationToken cancellationToken);
    }
}
