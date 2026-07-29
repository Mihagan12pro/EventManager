using OpenTelemetry.Exporter;

namespace Shared.AspNet.Options
{
    public class OtlpExporterOptions
    {
        public string EndPoint { get; set; }

        public OtlpExportProtocol Protocol { get; set; }

        public Uri EndPointUri
            => new Uri(EndPoint);
    }
}
