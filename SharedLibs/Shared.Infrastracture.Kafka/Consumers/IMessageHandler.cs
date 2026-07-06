namespace Shared.Infrastracture.Kafka.Consumers
{
    public interface IMessageHandler<in TMessage>
    {
        Task HandleAsync(
            TMessage message,
            CancellationToken cancellationToken);
    }
}
