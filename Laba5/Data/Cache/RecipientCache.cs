using Laba4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PostCity.Data.Cache;
using PostCity.Models;

namespace Laba4.Data.Cache
{
    public class RecipientCache : AppCache<Recipient>
    {
        public RecipientCache(PostCityContext db, IMemoryCache memoryCache) : base(db, memoryCache)
        {
        }

        public override IEnumerable<Recipient> Get()
        {
            _cache.TryGetValue("Recipient", out IEnumerable<Recipient>? emploees);

            if (emploees is null)
            {
                emploees = Set();
            }
            return emploees;
        }

        public override IEnumerable<Recipient> Set()
        {
            var recipients = _db.Recipients.Include(a => a.Address).Include(s => s.Subscriptions).ThenInclude(s => s.Publication).ToList();
            _cache.Set("Recipient", recipients, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
            return recipients;
        }

        public override IEnumerable<Recipient> Set(IEnumerable<Recipient> values)
        {
            _cache.Set("Recipient", values, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
            return values;
        }
    }
}
