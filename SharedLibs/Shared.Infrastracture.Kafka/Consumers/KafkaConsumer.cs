using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shared.Infrastracture.Kafka.SerializeDeserialize;
using Shared.Messaging;

namespace Shared.Infrastracture.Kafka.Consumers
{
    public class KafkaConsumer<TMessage> : BackgroundService
        where TMessage : IMessage
    {
        private readonly IConsumer<string, TMessage> _consumer; 
        private readonly IMessageHandler<TMessage> _messageHandler;
        private readonly string _topic;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
            => Task.Run(() => ConsumeAsync(stoppingToken), stoppingToken);  

        private async Task ConsumeAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = _consumer.Consume(stoppingToken);

                    await _messageHandler.HandleAsync(result.Message.Value, stoppingToken);
                }
            }
            catch(Exception)
            {
               
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Close();
            return base.StopAsync(cancellationToken);
        }

        public KafkaConsumer(
            IOptions<KafkaConsumerSettings> options,
            IMessageHandler<TMessage> messsageHandler)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = options.Value.BootstrapServers,

                Acks = Enum.Parse<Acks>(options.Value.Acks),

                GroupId = options.Value.GroupId
            };

            _topic = options.Value.Topic;

            _consumer = new ConsumerBuilder<string, TMessage>(config)
                .SetValueDeserializer(new KafkaJsonDeserializer<TMessage>())
                .Build();

            _messageHandler = messsageHandler;
        }
    }
}
