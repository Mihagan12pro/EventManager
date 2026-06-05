namespace EventManager.Handlers
{
    public interface ICommandHandler<T, TCommand>
    {
        Task<T> HandleAsync(
            in TCommand command,
            CancellationToken cancellationToken);
    }
}
