using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventsManager.Shared
{
    public static class UrlMaster
    {
        public static string CreateFromRequest(HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}{request.Path}";
        }

        public static string CreateWithoutPath(HttpRequest request, params object[] elements)
        {
            string uri = $"{request.Scheme}://{request.Host}";

            foreach (object element in elements)
            {
                uri += $"/{element}";
            }

            return uri;
        }

        public static string AddElementToEnd(string url, params object[] elements)
        {
            foreach (var element in elements)
            {
                url += $"/{element}";
            }
            return url;
        }
    }
}
