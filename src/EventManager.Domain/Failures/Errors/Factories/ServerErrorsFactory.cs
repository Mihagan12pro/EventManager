using System.Net;

namespace EventManager.Domain.Failures.Errors.Factories
{
    public static class ServerErrorsFactory
    {
        public static HttpErrorWorkbench InternalServerErrorWorkbench
            => new HttpErrorWorkbench(HttpStatusCode.InternalServerError);
    }
}
