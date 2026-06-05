namespace EventManager.Handlers
{
    public interface ICommandHandler<T, TCommand>
    {
        Task<T> HandleAsync(
            in TCommand command,
            CancellationToken cancellationToken);
    }

    public interface ICommandHandler<TCommand>
    {
        Task HandlerAsync(
            in TCommand command, 
            CancellationToken cancellationToken);
    }
}
