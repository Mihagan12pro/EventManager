using Microsoft.Extensions.Configuration;

namespace Shared.Objects.Classes.Options.Global
{
    /// <summary>
    /// Exctracts data from the Kafka section of the global.json file
    /// </summary>
    public class KafkaOptions : GlobalOptions
    {
        private readonly IConfigurationSection _kafkaSection;

        public IConfigurationSection First(string sectionName)
            => _kafkaSection.GetRequiredSection(sectionName);

        public IConfigurationSection FirstOrDefault(string sectionName)
            => _kafkaSection.GetSection(sectionName);

        public string BootstrapServers
            => First(nameof(BootstrapServers)).Value!; 

        public KafkaOptions()
        {
            _kafkaSection = globalConfiguration.GetRequiredSection(nameof(KafkaOptions));
        }
    }
}

