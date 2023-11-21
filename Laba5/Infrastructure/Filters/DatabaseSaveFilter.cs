using Microsoft.AspNetCore.Mvc.Filters;
using PostCity.Models;
using PostCity.Data.Cache;

namespace PostCity.Infrastructure.Filters
{
    public class DatabaseSaveFilter: IActionFilter
    {
        private readonly SubscriptionCache _appCache;

        public DatabaseSaveFilter(SubscriptionCache appCache)
        {
            _appCache = appCache;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

            _appCache.Set();
        }
    }
}
