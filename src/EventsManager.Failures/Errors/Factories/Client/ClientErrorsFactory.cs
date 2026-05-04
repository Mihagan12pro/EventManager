using System.Net;

namespace EventsManager.Failures.Errors.ErrorsFactory.Client
{
    public static class ClientErrorsFactory
    {
        public static HttpErrorWorkbench ConflictWorkbench
            => new HttpErrorWorkbench(HttpStatusCode.Conflict);

        public static HttpErrorWorkbench NotFoundWorkbench
            => new HttpErrorWorkbench(HttpStatusCode.NotFound);

        public static HttpErrorWorkbench BadRequestWorkbench
            => new HttpErrorWorkbench(HttpStatusCode.BadRequest);
    }
}
