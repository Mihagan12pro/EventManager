namespace Shared.Objects.Interfaces
{
    public interface ICommandHandler<TOutput, TCommand>
        where TCommand : ICommand
    {
        Task<TOutput> HandleAsync(
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
