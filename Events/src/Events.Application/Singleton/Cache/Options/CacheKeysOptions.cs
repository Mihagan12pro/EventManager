namespace Events.Application.Singleton.Cache.Options
{
    public class CacheKeysOptions
    {
        public readonly CacheKeyOptions TopEventsKey = new CacheKeyOptions();

        public readonly CacheKeyOptions GetEventKey = new CacheKeyOptions();
    }
}
