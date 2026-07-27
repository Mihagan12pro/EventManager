using Microsoft.Extensions.Configuration;

namespace Shared.Objects.Classes.Options.Global
{
    /// <summary>
    /// Extracts information from the global.json file
    /// </summary>
    public abstract class GlobalOptions
    {
        protected readonly IConfiguration globalConfiguration;

        public GlobalOptions()
        {
            globalConfiguration = new ConfigurationBuilder()
                                .AddJsonFile(new DirectoryInfo(@"..\..\..\global.json").FullName)
                                .Build();
        }
    }
}