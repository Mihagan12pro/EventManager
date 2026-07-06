using Microsoft.Extensions.Configuration;

namespace Shared.Objects.Classes.Options
{
    /// <summary>
    /// Exctracts data from the Kafka section of the global.json file
    /// </summary>
    public class KafkaOptions : GlobalOptions
    {
        private readonly IConfigurationSection _kafkaSection;

        public IConfigurationSection FirstProducer(string sectionName)
            => _kafkaSection.GetRequiredSection($"Producers:{sectionName}");

        public IConfigurationSection FirstProducerOrDefault(string sectionName)
            => _kafkaSection.GetSection($"Producers:{sectionName}");

        public IConfigurationSection FirstConsumer(string sectionName)
            => _kafkaSection.GetRequiredSection($"Consumers:{sectionName}");

        public IConfigurationSection FirstConsumerOrDefault(string sectionName)
            => _kafkaSection.GetSection($"Consumers:{sectionName}");

        public KafkaOptions()
        {
            _kafkaSection = globalConfiguration.GetRequiredSection("Kafka");
        }
    }
}

