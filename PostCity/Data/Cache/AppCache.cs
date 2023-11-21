using Microsoft.Extensions.Caching.Memory;
using PostCity.Data;

namespace PostCity.Data.Cache
{
    public abstract class AppCache<T>
    {
        protected readonly IMemoryCache _cache;
        protected readonly PostCityContext _db;

        internal readonly int _saveTime = 2 * 2 * 240;


        public AppCache(PostCityContext db, IMemoryCache memoryCache)
        {
            _db = db;
            _cache = memoryCache;
        }

        public abstract IEnumerable<T> Get();

        public abstract IEnumerable<T> Set();

        public abstract IEnumerable<T> Set(IEnumerable<T> values);

        public void Update()
        {
            Set();
        }
    }
}
