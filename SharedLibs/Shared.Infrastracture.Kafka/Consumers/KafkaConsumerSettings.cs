namespace Shared.Infrastracture.Kafka.Consumers
{
    public class KafkaConsumerSettings
    {
        public string Topic { get; set; }

        public string BootstrapServers { get; set; }

        public string GroupId { get; set; }
    }
}
