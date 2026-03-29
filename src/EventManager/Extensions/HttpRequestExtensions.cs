using System.Runtime.CompilerServices;

namespace EventManager.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string ToUrl(
            this HttpRequest request, 
            List<object> objs)
                => request.ToUrl(true, objs);

        public static string ToUrl(
            this HttpRequest request,
            bool withPath, 
            List<object> objs)
        {
            string url = $"{request.Scheme}://{request.Host}";

            if (withPath)
                url += $"{request.Path}";

            foreach (object obj in objs)
            {
                url += $"/{obj}";
            }

            return url;
        }
    }
}
