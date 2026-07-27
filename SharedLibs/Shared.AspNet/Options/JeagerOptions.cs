using Microsoft.Extensions.Configuration;
using Shared.Objects.Classes.Options.Global;
using System.Text.Json;

namespace Shared.AspNet.Options
{
    internal class JeagerOptions
        : GlobalOptions
    {
        public OtlpExportOptions ExportOptions { get; private set; }

        public BatchExportOptions BatchExportOptions { get; private set; }

        public JeagerOptions()
        {
            ExportOptions = JsonSerializer.Deserialize<OtlpExportOptions>(globalConfiguration.GetRequiredSection("JaegerOptions:OtlpExporterOptions").Value);

            BatchExportOptions = JsonSerializer.Deserialize<BatchExportOptions>(globalConfiguration.GetRequiredSection("JaegerOptions:BatchExportOptions").Value);
        }
    }
}
