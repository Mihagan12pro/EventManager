namespace Shared
{
    public class Error
    {
        public int StatusCode { get; }
        public string Message { get; }

        public static Error CreateError404(string message = "Not found!")
            => new Error(404, message);

        public static Error CreateError400 (string message = "Bad request!")
            => new Error(400, message);

        public static Error CreateError500(string message = "Internal server error!")
            => new Error(500, message);

        private Error(int statusCode, string message)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}
