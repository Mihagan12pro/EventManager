using System.Net;

namespace Shared.Failures.Errors.Factories
{
    public static class ServerErrorsFactory
    {
        public static HttpErrorWorkbench InternalServerErrorWorkbench
           => new HttpErrorWorkbench(HttpStatusCode.InternalServerError);

        public static HttpErrorWorkbench ServiceUnavailableWorkbench
           => new HttpErrorWorkbench(HttpStatusCode.ServiceUnavailable);
    }
}
