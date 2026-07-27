using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.AspNet.Options
{
    internal class BatchExportOptions
    {
        public int ScheduledDelayMilliseconds { get; set; }

        public int ExporterTimeoutMilliseconds { get; set; }
    }
}
