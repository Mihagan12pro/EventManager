namespace Shared.AspNet.Options
{
    internal class JaegerOptions
    {
        public required OtlpExportOptions OtlpExportOptions { get; set; }

        public required BatchExportOptions BatchExportOptions { get; set; }
    }
}
