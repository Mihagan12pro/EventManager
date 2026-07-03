using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Shared.Failures.Errors.Factories
{
    internal static class ClientErrorsFactory
    {
        public static HttpErrorWorkbench ConflictWorkbench
            => new HttpErrorWorkbench(HttpStatusCode.Conflict);

        public static HttpErrorWorkbench NotFoundWorkbench
            => new HttpErrorWorkbench(HttpStatusCode.NotFound);

        public static HttpErrorWorkbench BadRequestWorkbench
            => new HttpErrorWorkbench(HttpStatusCode.BadRequest);

        public static HttpErrorWorkbench ForbiddenWorkbench
            => new HttpErrorWorkbench(HttpStatusCode.Forbidden);
    }
}
