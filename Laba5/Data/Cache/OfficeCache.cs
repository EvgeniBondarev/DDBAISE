using Laba4.Models;
using Microsoft.Extensions.Caching.Memory;
using PostCity.Models;

namespace PostCity.Data.Cache
{
    public class OfficeCache : AppCache<Office>
    {
        public OfficeCache(SubsCityContext db, IMemoryCache memoryCache) : base(db, memoryCache)
        {
        }

        public override IEnumerable<Office> Get()
        {
            _cache.TryGetValue("Offices", out IEnumerable<Office>? offices);

            if (offices is null)
            {
                offices = Set();
            }
            return offices;
        }

        public override IEnumerable<Office> Set()
        {
            var offices = _db.Offices.ToList();
            _cache.Set("Offices", offices, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
            return offices;
        }

        public override IEnumerable<Office> Set(IEnumerable<Office> offices)
        {
            _cache.Set("Offices", offices, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
            return offices;
        }

    }
}
