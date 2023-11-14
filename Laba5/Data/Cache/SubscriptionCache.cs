using Laba4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Laba4.Data.Cache
{
    public class SubscriptionCache : AppCache<Subscription>
    {
        public SubscriptionCache(SubsCityContext subsCityContext, IMemoryCache memoryCache) : base(subsCityContext, memoryCache)
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
            var subscriptions = _subsCityContext.Subscriptions.Include(p => p.Publication).ThenInclude(t => t.Type).ToList();
            _cache.Set("Subscriptions", subscriptions, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
            return subscriptions;
        }
    }
}
