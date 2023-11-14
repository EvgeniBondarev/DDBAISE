using Laba4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;

namespace Laba4.Data.Cache
{
    public abstract class AppCache <T>
    {
        internal readonly IMemoryCache _cache;
        internal readonly SubsCityContext _subsCityContext;

        internal readonly int _saveTime = 2 * 2 * 240;


        public AppCache(SubsCityContext subsCityContext, IMemoryCache memoryCache)
        {
            _subsCityContext = subsCityContext;
            _cache = memoryCache;
        }

        public abstract IEnumerable<T> Get();

        public abstract IEnumerable<T> Set();
    }
}
