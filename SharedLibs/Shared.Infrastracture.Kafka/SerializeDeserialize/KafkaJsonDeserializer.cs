using System.Text.Json;
using Confluent.Kafka;

namespace Shared.Infrastracture.Kafka.SerializeDeserialize
{
    public class KafkaJsonDeserializer<TMessage> : IDeserializer<TMessage>
    {
        public TMessage Deserialize(
            ReadOnlySpan<byte> data,
            bool isNull,
            SerializationContext context)
                => JsonSerializer.Deserialize<TMessage>(data)!;
    }
}
