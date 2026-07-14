using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Shared.Infrastracture.Kafka.SerializeDeserialize
{
    public class KafkaJsonSerializer<TMessage> : ISerializer<TMessage>
    {
        public byte[] Serialize(TMessage data, SerializationContext context)
            => JsonSerializer.SerializeToUtf8Bytes(data);
    }
}
