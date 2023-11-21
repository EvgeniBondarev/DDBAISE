using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PostCity.Data;
using PostCity.Models;

namespace PostCity.Data.Cache
{
    public class SubscriptionCache : AppCache<Subscription>
    {
        public SubscriptionCache(PostCityContext db, IMemoryCache memoryCache) : base(db, memoryCache)
        {
        }

        public override IEnumerable<Subscription> Get()
        {
            _cache.TryGetValue("Subscriptions", out IEnumerable<Subscription>? subscriptions);

            if (subscriptions is null)
            {
                subscriptions = Set();
            }
            return subscriptions;
        }

        public override IEnumerable<Subscription> Set()
        {
            var subscriptions = _db.Subscriptions.Include(s => s.Employee)
                                                 .Include(s => s.Office)
                                                 .Include(s => s.Publication)
                                                 .Include(s => s.Recipient).ToList();
            _cache.Set("Subscriptions", subscriptions, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
            return subscriptions;
        }

        public override IEnumerable<Subscription> Set(IEnumerable<Subscription> subscriptions)
        {
            _cache.Set("Subscriptions", subscriptions, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
            return subscriptions;
        }


    }
}
