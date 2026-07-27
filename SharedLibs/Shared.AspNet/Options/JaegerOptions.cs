using Microsoft.Extensions.Configuration;
using OpenTelemetry.Exporter;
using Shared.Objects.Classes.Options.Global;
using System.Text.Json;

namespace Shared.AspNet.Options
{
    internal class JaegerOptions
        : GlobalOptions
    {
        public OtlpExportOptions ExportOptions { get; private set; }

        public BatchExportOptions BatchExportOptions { get; private set; }

        public JaegerOptions()
        {
            var jaegerSection = globalConfiguration.GetRequiredSection("JaegerOptions");

            ExportOptions = jaegerSection.GetSection("OtlpExporterOptions").Get<OtlpExportOptions>();
            BatchExportOptions = jaegerSection.GetSection("BatchExportOptions").Get<BatchExportOptions>();
        }
    }
}
