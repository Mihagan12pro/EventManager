using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Shared.Infrastracture.Kafka.Consumers
{
    public class KafkaConsumer<TMessage> : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        public KafkaConsumer(IOptions<KafkaConsumerSettings> options)
        {
            
        }
    }
}
