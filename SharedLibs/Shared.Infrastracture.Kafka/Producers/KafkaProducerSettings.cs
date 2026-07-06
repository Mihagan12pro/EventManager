namespace Shared.Infrastracture.Kafka.Producers
{
    public class KafkaProducerSettings
    {
        public string Topic { get; set;  }

        public string BootstrapServers { get; set; }

        public string Acks { get; set; }
    }
}
