using System.Net;

namespace Shared.Failures.Errors.Factories
{
    public static class ServerErrorsFactory
    {
        public static HttpErrorWorkbench InternalServerErrorWorkbench
           => new HttpErrorWorkbench(HttpStatusCode.InternalServerError);
    }
}
