using Shared.Messaging;

namespace Shared.Infrastracture.Kafka.Producers
{
    public interface IKafkaProducer<in TMessage> : IDisposable
        where TMessage : IMessage
    {
        Task ProduceAsync(
            TMessage message, 
            CancellationToken cancellationToken);
    }
}
