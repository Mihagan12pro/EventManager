namespace EventManager
{
    public static class UrlMaster
    {
        public static string CreateFromRequest(HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}{request.Path}";
        }

        public static string AddElementToEnd(string url, object element)
        {
            return $"{url}/{element}";
        }
    }
}
