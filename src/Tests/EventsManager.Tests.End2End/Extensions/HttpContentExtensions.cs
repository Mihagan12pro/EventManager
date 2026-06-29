namespace EventsManager.Tests.End2End.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<Guid> ExtractGuid(this HttpContent content)
        {
            string str = await content.ReadAsStringAsync();

            
            return new Guid(str.Trim('"'));
        }
    }
}
