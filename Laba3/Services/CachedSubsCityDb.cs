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
       
        public void AddEmployeeToCache(CacheKey key, int rowsNumber = 20)
        {
            if(!_memoryCache.TryGetValue(key, out IEnumerable<Employee> cachedEmployee)) {
                cachedEmployee = _dbContext.Employees.Take(rowsNumber).ToList();

                if (cachedEmployee != null)
                {
                    _memoryCache.Set(key, cachedEmployee, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_saveTime)
                    });
                }
                Console.WriteLine("Таблица Employee занесена в кеш");
            }
            else {
                Console.WriteLine("Таблица Employee уже находится в кеше");
            }
        }

        public IEnumerable<Employee> GetEmployee(CacheKey key, int rowsNumber = 20)
        {
            IEnumerable<Employee> employes;
            if (!_memoryCache.TryGetValue(key, out employes))
            {
                employes = _dbContext.Employees.Take(rowsNumber).ToList();
                if (employes != null)
                {
                    _memoryCache.Set(key, employes,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
                }
            }
            return employes;
        }

        public void AddEmployeePositionToCache(CacheKey key, int rowsNumber = 20)
        {
            if (!_memoryCache.TryGetValue(key, out IEnumerable<EmployeePosition> cachedEmployeePositions))
            {
                cachedEmployeePositions = _dbContext.EmployeePositions.Take(rowsNumber).ToList();

                if (cachedEmployeePositions != null)
                {
                    _memoryCache.Set(key, cachedEmployeePositions, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_saveTime)
                    });
                }
                Console.WriteLine("Таблица EmployeePositions занесена в кеш");
            }
            else
            {
                Console.WriteLine("Таблица EmployeePositions уже находится в кеше");
            }
        }

        public IEnumerable<EmployeePosition> GetEmployeePosition(CacheKey key, int rowsNumber = 20)
        {
            IEnumerable<EmployeePosition> employeePositions;
            if (!_memoryCache.TryGetValue(key, out employeePositions))
            {
                employeePositions = _dbContext.EmployeePositions.Take(rowsNumber).ToList();
                if (employeePositions != null)
                {
                    _memoryCache.Set(key, employeePositions,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
                }
            }
            return employeePositions;
        }

        public void AddOfficeToCache(CacheKey key, int rowsNumber = 20)
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
        public IEnumerable<Office> GetOffice(CacheKey key, int rowsNumber = 20)
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

        public void AddPuplicationToCache(CacheKey key, int rowsNumber = 20)
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
        public IEnumerable<Publication> GetPublication(CacheKey key, int rowsNumber = 20)
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

        public void AddPuplicationTypeToCache(CacheKey key, int rowsNumber = 20)
        {
            if (!_memoryCache.TryGetValue(key, out IEnumerable<PublicationType> cachedPublicationTypes))
            {
                cachedPublicationTypes = _dbContext.PublicationTypes.Take(rowsNumber).ToList();

                if (cachedPublicationTypes != null)
                {
                    _memoryCache.Set(key, cachedPublicationTypes, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_saveTime)
                    });
                }
                Console.WriteLine("Таблица PublicationType занесена в кеш");
            }
            else
            {
                Console.WriteLine("Таблица PublicationTypes уже находится в кеше");
            }
        }

        public IEnumerable<PublicationType> GetPublicationType(CacheKey key, int rowsNumber = 20)
        {
            IEnumerable<PublicationType> publicationTypes;
            if (!_memoryCache.TryGetValue(key, out publicationTypes))
            {
                publicationTypes = _dbContext.PublicationTypes.Take(rowsNumber).ToList();
                if (publicationTypes != null)
                {
                    _memoryCache.Set(key, publicationTypes,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
                }
            }
            return publicationTypes;
        }

        public void AddRecipientToCache(CacheKey key, int rowsNumber = 20)
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

        public IEnumerable<Recipient> GetRecipient(CacheKey key, int rowsNumber = 20)
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

        public void AddRecipientAddressToCache(CacheKey key, int rowsNumber = 20)
        {
            if (!_memoryCache.TryGetValue(key, out IEnumerable<RecipientAddress> cachedRecipientAddress))
            {
                cachedRecipientAddress = _dbContext.RecipientAddresses.Take(rowsNumber).ToList();

                if (cachedRecipientAddress != null)
                {
                    _memoryCache.Set(key, cachedRecipientAddress, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_saveTime)
                    });
                }
                Console.WriteLine("Таблица RecipientAddress занесена в кеш");
            }
            else
            {
                Console.WriteLine("Таблица RecipientAddress уже находится в кеше");
            }
        }

        public IEnumerable<RecipientAddress> GetRecipientAddress(CacheKey key, int rowsNumber = 20)
        {
            IEnumerable<RecipientAddress> recipientAddresses;
            if (!_memoryCache.TryGetValue(key, out recipientAddresses))
            {
                recipientAddresses = _dbContext.RecipientAddresses.Take(rowsNumber).ToList();
                if (recipientAddresses != null)
                {
                    _memoryCache.Set(key, recipientAddresses,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
                }
            }
            return recipientAddresses;
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
