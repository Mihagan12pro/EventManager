namespace Events.Application.Singleton.Cache.Options
{
    public class CacheKeyOptions
    {
        /// <summary>
        /// TTL in seconds
        /// </summary>
        public int TTL { get; set; }

        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Expiry in seconds
        /// </summary>
        public TimeSpan Expiry
            => TimeSpan.FromSeconds(TTL);

        public string FormatKey(params object[] values)
            => string.Format(Key, values);
    }
}
