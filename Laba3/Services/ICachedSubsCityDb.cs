namespace Laba3.Services
{
    public interface ICachedSubsCityDb
    {
        void AddOfficeToCache(CacheKey key, int rowsNumber = 100);
        IEnumerable<Office> GetOffice(CacheKey key, int rowsNumber = 100);

        void AddPuplicationToCache(CacheKey key, int rowsNumber = 100);
        IEnumerable<Publication> GetPublication(CacheKey key, int rowsNumber = 100);

        void AddRecipientToCache(CacheKey key, int rowsNumber = 100);
        IEnumerable<Recipient> GetRecipient(CacheKey key, int rowsNumber = 100);

        void AddSubscriptionToCache(CacheKey key, int rowsNumber = 20);
        IEnumerable<Subscription> GetSubscription(CacheKey key, int rowsNumber = 20);
    }

}
}
