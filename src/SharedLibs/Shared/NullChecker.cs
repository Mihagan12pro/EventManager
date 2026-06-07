using EventManager.Services.Exceptions.WebApi.Client.NotFound;

namespace EventsManager.Shared
{
    public static class NullChecker
    {
        /// <summary>
        /// Throws NotFoundException if obj is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        /// <exception cref="NotFoundException"></exception>
        public static void Check<T>(T? obj, string message = "This resource does not exists!")
        {
            if (obj == null)
                throw new NotFoundException(message);
        }
    }
}
