using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Repository.Models;

namespace Service.Data.Cache
{
    public class PublicationCache : AppCache<Publication>
    {
        public PublicationCache(PostCityContext db, IMemoryCache memoryCache) : base(db, memoryCache)
        {
        }

        public override IEnumerable<Publication> Get()
        {
            _cache.TryGetValue("Publication", out IEnumerable<Publication>? publications);

            if (publications is null)
            {
                publications = Set();
            }
            return publications;
        }

        public override IEnumerable<Publication> Set()
        {
            var subscriptions = _db.Publications.Include(t => t.Type).ToList();
            _cache.Set("Publication", subscriptions, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
            return subscriptions;
        }

        public override IEnumerable<Publication> Set(IEnumerable<Publication> values)
        {
            _cache.Set("Publication", values, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
            return values;
        }
    }
}
