using EventsManager.Failures.Errors.ErrorsFactory;
using System.Net;

namespace EventsManager.Failures.Errors.Factories.Server
{
    public static class ServerErrorsFactory
    {
        public static HttpErrorWorkbench InternalServerErrorWorkbench
            => new HttpErrorWorkbench(HttpStatusCode.InternalServerError);
    }
}
