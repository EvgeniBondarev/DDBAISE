using PostCity.Data.Cache;

namespace Laba4.Data.Cache
{
    public class CacheUpdater
    {
        private EmployeeCache _employeeCache;
        private OfficeCache _officeCache;
        private PublicationCache _publicationCache;
        private RecipientCache _recipientCache;
        private SubscriptionCache _subscriptionCache;
        private UserCache _userCache;

        public CacheUpdater(RecipientCache recipientCache, SubscriptionCache subscriptionCache, UserCache userCache,
                            EmployeeCache employeeCache, PublicationCache publicationCache, OfficeCache officeCache)
        {
            _recipientCache = recipientCache;
            _subscriptionCache = subscriptionCache;
            _userCache = userCache;
            _employeeCache = employeeCache;
            _officeCache = officeCache;
            _publicationCache = publicationCache;
        }

        public void Update(IAppCache cacheEntity)
        {
            cacheEntity.Update();


            if (typeof(OfficeCache).IsAssignableFrom(cacheEntity.GetType()))
            {
                _employeeCache.Update();
                _subscriptionCache.Update();
            }
            if (typeof(EmployeeCache).IsAssignableFrom(cacheEntity.GetType()))
            {
                _officeCache.Update();
                _subscriptionCache.Update();
                _userCache.Update();
            }
            if (typeof(PublicationCache).IsAssignableFrom(cacheEntity.GetType()))
            {
                _subscriptionCache.Update();
            }
            if (typeof(RecipientCache).IsAssignableFrom(cacheEntity.GetType()))
            {
                _subscriptionCache.Update();
                _userCache.Update();
            }
            if (typeof(UserCache).IsAssignableFrom(cacheEntity.GetType()))
            {
                _employeeCache.Update();
                _recipientCache.Update();
            }

        }
    }
}
