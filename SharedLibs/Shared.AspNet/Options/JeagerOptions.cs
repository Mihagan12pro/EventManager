namespace Shared.AspNet.Options
{
    public class JeagerOptions
    {
        public required OtlpExporterOptions OtlpExportOptions { get; set; }

        public required BatchExportOptions BatchExportOptions { get; set; }
    }
}
