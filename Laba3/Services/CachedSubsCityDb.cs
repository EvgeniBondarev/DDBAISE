using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Laba3.Services
{
    public class CachedSubsCityDb
    {
        private readonly SubsCityContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly int _saveTime;
        public CachedSubsCityDb(SubsCityContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
            _saveTime = 2 * 2 + 240;
        }
       
     

        public void AddOfficeToCache(CacheKey key, int rowsNumber = 100)
        {
            if (!_memoryCache.TryGetValue(key, out IEnumerable<Office> cachedOffice))
            {
                cachedOffice = _dbContext.Offices.Take(rowsNumber).ToList();

                if (cachedOffice != null)
                {
                    _memoryCache.Set(key, cachedOffice, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_saveTime)
                    });
                }
                Console.WriteLine("Таблица Office занесена в кеш");
            }
            else
            {
                Console.WriteLine("Таблица Office уже находится в кеше");
            }
        }
        public IEnumerable<Office> GetOffice(CacheKey key, int rowsNumber = 100)
        {
            IEnumerable<Office> offices;
            if (!_memoryCache.TryGetValue(key, out offices))
            {
                offices = _dbContext.Offices.Take(rowsNumber).ToList();
                if (offices != null)
                {
                    _memoryCache.Set(key, offices,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
                }
            }
            return offices;
        }

        public void AddPuplicationToCache(CacheKey key, int rowsNumber = 100)
        {
            if (!_memoryCache.TryGetValue(key, out IEnumerable<Publication> cachedPublication))
            {
                cachedPublication = _dbContext.Publications.Take(rowsNumber).ToList();

                if (cachedPublication != null)
                {
                    _memoryCache.Set(key, cachedPublication, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_saveTime)
                    });
                }
                Console.WriteLine("Таблица Puplication занесена в кеш");
            }
            else
            {
                Console.WriteLine("Таблица Puplication уже находится в кеше");
            }
        }
        public IEnumerable<Publication> GetPublication(CacheKey key, int rowsNumber = 100)
        {
            IEnumerable<Publication> publications;
            if (!_memoryCache.TryGetValue(key, out publications))
            {
                publications = _dbContext.Publications.Take(rowsNumber).ToList();
                if (publications != null)
                {
                    _memoryCache.Set(key, publications,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
                }
            }
            return publications;
        }

        public void AddRecipientToCache(CacheKey key, int rowsNumber = 100)
        {
            if (!_memoryCache.TryGetValue(key, out IEnumerable<Recipient> cachedRecipient))
            {
                cachedRecipient = _dbContext.Recipients.Take(rowsNumber).ToList();

                if (cachedRecipient != null)
                {
                    _memoryCache.Set(key, cachedRecipient, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_saveTime)
                    });
                }
                Console.WriteLine("Таблица Recipients занесена в кеш");
            }
            else
            {
                Console.WriteLine("Таблица Recipients уже находится в кеше");
            }
        }

        public IEnumerable<Recipient> GetRecipient(CacheKey key, int rowsNumber = 100)
        {
            IEnumerable<Recipient> recipients;
            if (!_memoryCache.TryGetValue(key, out recipients))
            {
                recipients = _dbContext.Recipients.Take(rowsNumber).ToList();
                if (recipients != null)
                {
                    _memoryCache.Set(key, recipients,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
                }
            }
            return recipients;
        }

  

        public void AddSubscriptionToCache(CacheKey key, int rowsNumber = 20)
        {
            if (!_memoryCache.TryGetValue(key, out IEnumerable<Subscription> cachedSubscription))
            {
                cachedSubscription = _dbContext.Subscriptions.Take(rowsNumber).ToList();

                if (cachedSubscription != null)
                {
                    _memoryCache.Set(key, cachedSubscription, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_saveTime)
                    });
                }
                Console.WriteLine("Таблица Subscription занесена в кеш");
            }
            else
            {
                Console.WriteLine("Таблица Subscription уже находится в кеше");
            }
        }

        public IEnumerable<Subscription> GetSubscription(CacheKey key, int rowsNumber = 20)
        {
            IEnumerable<Subscription> subscriptions;
            if (!_memoryCache.TryGetValue(key, out subscriptions))
            {
                subscriptions = _dbContext.Subscriptions.Take(rowsNumber).ToList();
                if (subscriptions != null)
                {
                    _memoryCache.Set(key, subscriptions,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
                }
            }
            return subscriptions;
        }
    }
}
