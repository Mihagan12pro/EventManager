namespace Shared.AspNet.Options
{
    public class JaegerOptions
    {
        public required OtlpExporterOptions OtlpExportOptions { get; set; }

        public required BatchExportOptions BatchExportOptions { get; set; }
    }
}
