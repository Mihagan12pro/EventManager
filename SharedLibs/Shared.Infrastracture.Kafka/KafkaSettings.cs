namespace Shared.Infrastracture.Kafka
{
    public class KafkaSettings
    {
        public string Topic { get; set;  }

        public string BootstrapServers { get; set; }

        public string Acks { get; set; }
    }
}
