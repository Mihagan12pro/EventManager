using System.Net;

namespace EventsManager.Failures.Errors
{
    /// <summary>
    /// Errors 4**
    /// </summary>
    public static class ClientErrors
    {
        /// <summary>
        /// Create bad request
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static HttpError Create400(string message = "Bad request!")
            => new HttpError(HttpStatusCode.BadRequest, message);

        /// <summary>
        /// Create not found
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static HttpError Create404(string message = "Not found!")
            => new HttpError(HttpStatusCode.NotFound, message);

        /// <summary>
        /// Create conflict
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static HttpError Create409(string message = "Conflict!")
            => new HttpError(HttpStatusCode.Conflict, message);
    }
}
