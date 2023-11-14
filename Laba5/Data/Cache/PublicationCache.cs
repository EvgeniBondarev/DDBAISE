using Laba4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Laba4.Data.Cache
{
    public class PublicationCache : AppCache<Publication>
    {
        public PublicationCache(SubsCityContext subsCityContext, IMemoryCache memoryCache) 
            : base(subsCityContext, memoryCache)
        {
        }

        public override IEnumerable<Publication> Get()
        {
            _cache.TryGetValue("Publications", out IEnumerable<Publication>? publications);

            if (publications is null)
            {
                publications = Set();
            }
            return publications;
        }

        public override IEnumerable<Publication> Set()
        {
            var publications = _subsCityContext.Publications.Include(t => t.Type).ToList();
            _cache.Set("Publications", publications, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
            return publications;
        }
    }
}
