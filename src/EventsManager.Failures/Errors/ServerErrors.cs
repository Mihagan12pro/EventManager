using System.Net;

namespace EventsManager.Failures.Errors
{
    /// <summary>
    /// Errors 5**
    /// </summary>
    public static class ServerErrors
    {
        /// <summary>
        /// Create internal server error
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static HttpError Create500(string message = "Internal server error!") 
            => new HttpError(HttpStatusCode.InternalServerError, message);
    }
}
