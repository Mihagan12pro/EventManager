namespace EventManager.Shared
{
    public class Error
    {
        public readonly StatusCodes StatusCode;
        public readonly string Message;

        public static Error CreateError404(string message = "Not found!")
            => new Error(StatusCodes.NotFound, message);

        public static Error CreateError400 (string message = "Bad request!")
            => new Error(StatusCodes.BadRequest, message);

        public static Error CreateError500(string message = "Internal server error!")
            => new Error(StatusCodes.InternalServerError, message);

        private Error(StatusCodes statusCode, string message)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}
